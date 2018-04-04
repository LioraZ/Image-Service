using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string outputFolder;            // The Output Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        public string AddFile(string path, out bool result)
        {
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
                    return "New folder created successfully!";
                }
                catch (IOException e)
                {
                    result = false;
                    return "An exception ocuured./n Please try again";
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
                File.Copy(src, outputFolder);
                result = true;
                return "File successfully added!";
            }
            catch (Exception e)
            {
                result = false;
                return "File exists at output folder!";

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
    }
}