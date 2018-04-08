using ImageService.Infrastructure;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string outputFolder;            // The Output Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        public ImageServiceModel(string outputDir, int imageThumbnailSize)
        {
            outputFolder = outputDir;
            thumbnailSize = imageThumbnailSize;
            bool result;
            CreateFolder(outputFolder, out result);
            //if (!result) 
        }
        public string AddFile(string path, out bool result)
        {
            string folderName = ParseDate(DateTaken(path));
            //bool result;
            string errorMsg = CreateFolder(folderName, out result);
            if (!result) return errorMsg;
            return MoveFile(path, outputFolder, out result);
        }
        public string CreateFolder(string path, out bool result)
        {
           PathExists(path, out result);
           if (!result)
           {
                try
                {
                    Directory.CreateDirectory(path);
                    result = true;
                    return path;
                    //return "New folder created successfully!";
                }
                catch (IOException e)
                {
                    result = false;
                    return e.Message;
                    //return "An exception ocuured./n Please try again";
                }
                
           }
            result = true;
            return "Folder already exists!";

        }
        public string MoveFile(string src, string dst, out bool result)
        {
            string errorMsg = OutputFolderExists(dst, out result);
            if (!result) { return errorMsg; }
            errorMsg = PathExists(src, out result);
            if (!result) { return errorMsg; }
            //check edge cases
            try
            {
                File.Move(src, dst);
                result = true;
                return "File successfully added!";
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
                //return "File exists at output folder!";
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
            Regex r = new Regex(":");
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        private string ParseDate(DateTime date)
        {
            return date.Year.ToString() + date.Month.ToString();
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //DateTime date = DateTaken(e.Name);
            string folderName = ParseDate(DateTaken(e.Name));
            bool result;
            CreateFolder(folderName, out result);
            //CreateFolder(outputFolder + "/" + folderName, out result);
            if (!result) return;
            


            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
    }
}