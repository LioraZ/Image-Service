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
            outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
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
                if (File.Exists(dst)) File.Delete(dst);
                File.Move(src, dst);
                CreateThumbnail(dst, out result);
                return dst;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        public void CreateThumbnail(string fileName, out bool result)
        {
            Image image = Image.FromFile(fileName);
            Image thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);
            string thumbnailPath = fileName.Replace(outputFolder, outputFolder + "\\Thumbnails");
            string thumbnailFolder = thumbnailPath.Replace("\\" + Path.GetFileName(thumbnailPath), "");
            CreateFolder(thumbnailFolder, out result);//should check for !result
            thumb.Save(Path.ChangeExtension(thumbnailPath, "thumb"));
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
            return date.Year.ToString() + "\\" + GetMonth(date.Month);
        }
        private string GetMonth(int month)
        {
            switch(month)
            {
                case (1): return "Jan";
                case (2): return "Feb";
                case (3): return "Mar";
                case (4): return "Apr";
                case (5): return "May";
                case (6): return "Jun";
                case (7): return "Jul";
                case (8): return "Aug";
                case (9): return "Sep";
                case (10): return "Oct";
                case (11):return "Nov";
                case (12): return "Dec";
            }
            return "";
        }
    }
}