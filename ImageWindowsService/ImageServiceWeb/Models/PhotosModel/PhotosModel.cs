using ImageServiceWeb.Models.Communication;
using ImageServiceWeb.Models.Config;
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

        public PhotoInfo[] Photos { get; set; }

        public PhotosModel(string outputDir)
        {
            OutputDir = outputDir;
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
                        photoThumbnail = photoThumbnail.Replace(OutputDir, thumbnailPath);
                        Path.ChangeExtension(photoThumbnail, Path.GetExtension(photo));
                        photoInfo.ThumbnailPath = photoThumbnail.Replace(".thumb", Path.GetExtension(photo));
                        photoInfo.RelativeThumbnailPath = MakeRelative(photoInfo.ThumbnailPath);
                        photoInfo.Name = Path.GetFileName(photo);
                        photoInfo.Date = month + " " + year;
                        tempPhotos.Add(photoInfo);
                    }
                }
            }
            Photos = tempPhotos.ToArray<PhotoInfo>();
        }

        private string MakeRelative(string filePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(OutputDir);
            return "~/" + referenceUri.MakeRelativeUri(fileUri).ToString();
        }

        public PhotoInfo GetPhotoInfo(string photoName)
        {
            foreach (PhotoInfo photoInfo in Photos) { if (photoInfo.Name == photoName) return photoInfo; }
            return null;
        }

        public void RemovePhoto(string photoName)
        {
            PhotoInfo photo = null;
            foreach (PhotoInfo photoInfo in Photos)
            {
                if (photoInfo.Name == photoName)
                {
                    photo = photoInfo;
                    DeletePhoto(photo);
                    break;
                }
            }
            List<PhotoInfo> tempPhotos = new List<PhotoInfo>(Photos);
            if (tempPhotos.Contains(photo)) tempPhotos.Remove(photo);
            Photos = tempPhotos.ToArray();
        }

        private void DeletePhoto(PhotoInfo photo)
        {
            try
            {
                File.Delete(photo.FullPath);
                File.Delete(photo.ThumbnailPath);
            }
            catch { }
        }
    }
}