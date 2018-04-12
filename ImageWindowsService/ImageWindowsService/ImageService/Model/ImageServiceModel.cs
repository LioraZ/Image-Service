using ImageService.Infrastructure;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Drawing.Imaging;
using System.Drawing;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string outputFolder;            // The Output Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        public ImageServiceModel()
        {
            outputFolder = ConfigurationSettings.AppSettings["OutputDir"];
            thumbnailSize = int.Parse(ConfigurationSettings.AppSettings["ThumbnailSize"]);
            bool result;
            CreateFolder(outputFolder, out result);
            CreateFolder(outputFolder + "\\Thumbnails", out result);
            //if (!result) 
        }
        public string AddFile(string path, out bool result)
        {
            string folderName = outputFolder + "\\" + ParseDate(DateTaken(path));
            string errorMsg = CreateFolder(folderName, out result);
            if (!result) return errorMsg;
            return MoveFile(path, folderName, out result);
        }
        public string CreateFolder(string path, out bool result)
        {
            try
            {
                Directory.CreateDirectory(path);
                result = true;
                return path;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
        public string MoveFile(string src, string dstFolder, out bool result)
        {
            string dst = dstFolder + "\\" + Path.GetFileName(src);
            try
            {
                if (File.Exists(dst)) File.Delete(src);
                else File.Move(src, dst);
                result = true;
                return dst;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
        
        private string OutputFolderExists(string dir, out bool result) {
            result = Directory.Exists(dir);
            return "Output folder not found at path!";
        }
        private string PathExists(string path, out bool result)
        {
            result = File.Exists(path);
            return "Image not found at path!";
        }


        public static DateTime DateTaken(string path)
        {
           try
            {
                Regex r = new Regex(":");
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            catch
            {
                return File.GetCreationTime(path);
            }
        }

        private string ParseDate(DateTime date)
        {
            return date.Year.ToString() + "\\" + date.Month.ToString();
        }
    }
}