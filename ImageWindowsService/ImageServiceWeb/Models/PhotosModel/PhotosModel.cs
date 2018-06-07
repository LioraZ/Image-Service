using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models.PhotosModel
{
    public class PhotosModel
    {
        public string OutputDir { get; set; }
        public string[] ThumbnailPaths { get; set; }
        //Uri uriAddress { get; set; }

        public PhotoInfo[] Photos { get; set; }

        public PhotosModel()
        {
            OutputDir = "C:\\Users\\Liora\\Documents\\ImageService\\ImageWindowsService\\ImageServiceWeb\\OutputDir";
            List<string> yearDirs = new List<string>(Directory.GetDirectories(OutputDir));
            string thumbnailPath = OutputDir + "\\Thumbnails";
            yearDirs.Remove(thumbnailPath);

            List<PhotoInfo> tempPhotos = new List<PhotoInfo>();
            foreach (string yearDir in yearDirs)
            {
                string year;
                year = Path.GetFileName(yearDir);
                string[] monthDirs = Directory.GetDirectories(yearDir);
                foreach (string monthDir in monthDirs)
                {
                    string month = Path.GetFileName(monthDir);
                    string[] photosDir = Directory.GetFiles(monthDir);
                    foreach (string photo in photosDir)
                    {
                        PhotoInfo photoInfo = new PhotoInfo();
                        photoInfo.FullPath = photo;
                        photoInfo.RelativePath = MakeRelative(photo);
                        string photoThumbnail = Path.ChangeExtension(photo, "thumb");
                        photoInfo.ThumbnailPath = photoThumbnail.Replace(OutputDir, thumbnailPath);
                        photoInfo.RelativeThumbnailPath = MakeRelative(photoInfo.ThumbnailPath);
                        photoInfo.Name = Path.GetFileName(photo);
                        photoInfo.Date = month + year;
                        tempPhotos.Add(photoInfo);
                    }
                }
            }
            Photos = tempPhotos.ToArray<PhotoInfo>();
            /**ThumbnailPaths = Directory.GetFiles(OutputDir, "*.thumb", SearchOption.AllDirectories);
            uriAddress = new Uri("file://server/filename.ext");
            
            for (int i = 0; i < ThumbnailPaths.Length; i++)
            {
                ThumbnailPaths[i] = HttpContext.Current.Server.MapPath(ThumbnailPaths[i]);
                ThumbnailPaths[i] = MakeRelative(ThumbnailPaths[i], "C:\\Users\\Liora\\Documents\\ImageService\\ImageWindowsService\\ImageServiceWeb\\Views\\Home");
            }*/
        }

        public string MakeRelative(string filePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(OutputDir);
            return "~/" + referenceUri.MakeRelativeUri(fileUri).ToString();
        }
    }
}