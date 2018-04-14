using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string outputFolder;            // The Output Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        /// <summary>
        /// The ImageServiceModel's constructor.
        /// </summary>
        public ImageServiceModel()
        {
            outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
          //  bool result;
          //  CreateFolder(outputFolder, out result);
          //  CreateFolder(outputFolder + "\\Thumbnails", out result);
        }

        /// <summary>
        /// The method adds the moves the given file to the output folder.
        /// </summary>
        /// <param name="path">The given path</param>
        /// <param name="result">True if the move was successful, and else false</param>
        /// <returns>A string with the path or an error message</returns>
        public string AddFile(string path, out bool result)
        {
            string folderName = outputFolder + "\\" + ParseDate(DateTaken(path));
            string errorMsg = CreateFolder(folderName, out result);
            if (!result) return errorMsg;
            return MoveFile(path, folderName, out result);
        }

        /// <summary>
        /// The method crreates a new folder given the path.
        /// </summary>
        /// <param name="path">The given path</param>
        /// <param name="result">The action's result</param>
        /// <returns>A string with the path or an error message</returns>
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

        /// <summary>
        /// The method tries to delete a given file.
        /// </summary>
        /// <param name="path">The given file</param>
        public void TryDeleteFile(string path)
        {
            const int NumberOfRetries = 3;
            const int DelayOnRetry = 1000;
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    File.Delete(path);
                    break;
                }
                catch when (i <= NumberOfRetries)
                {
                    Thread.Sleep(DelayOnRetry);
                }
            }
        }

        /// <summary>
        /// The method moves a file from its source to its destination.
        /// </summary>
        /// <param name="src">he file's source</param>
        /// <param name="dstFolder">The file's destination folder</param>
        /// <param name="result">>The actin's result</param>
        /// <returns>A string with the path or an error message</returns>
        public string MoveFile(string src, string dstFolder, out bool result)
        {
            string dst = dstFolder + "\\" + Path.GetFileName(src);
            try
            {
                if (File.Exists(dst)) TryDeleteFile(dst);
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

        /// <summary>
        /// The method creates a thumbnail for the file/picture.
        /// </summary>
        /// <param name="fileName">Thr given file/image</param>
        /// <param name="result">The action's result</param>
        private void CreateThumbnail(string fileName, out bool result)
        {
            Image image = Image.FromFile(fileName);
            Image thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);
            string thumbnailPath = fileName.Replace(outputFolder, outputFolder + "\\Thumbnails");
            string thumbnailFolder = thumbnailPath.Replace("\\" + Path.GetFileName(thumbnailPath), "");
            CreateFolder(thumbnailFolder, out result);//should check for !result
            thumb.Save(Path.ChangeExtension(thumbnailPath, "thumb"));
        }

        /// <summary>
        /// The method return the DateTime of the file.
        /// </summary>
        /// <param name="path">The given file's path</param>
        /// <returns>The extracted DateTime</returns>
        private DateTime DateTaken(string path)
        {
           try
            {
                Regex r = new Regex(":");
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);//306
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            catch
            {
                return File.GetCreationTime(path);
            }
        }

        /// <summary>
        /// The method parses the dateTime received to a directory extension.
        /// </summary>
        /// <param name="date">The DateTime received</param>
        /// <returns>The parsed directory extension</returns>
        private string ParseDate(DateTime date)
        { 
            return date.Year.ToString() + "\\" + GetMonth(date.Month);
        }

        /// <summary>
        /// The method returns the month name given its numeric representation.
        /// </summary>
        /// <param name="month">The month in its numeric representation</param>
        /// <returns>The month's name</returns>
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