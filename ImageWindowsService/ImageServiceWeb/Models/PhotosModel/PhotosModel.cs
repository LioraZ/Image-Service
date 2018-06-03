using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models.PhotosModel
{
    public class PhotosModel
    {
        public string OutpurDir { get; set; }
        public string[] ThumbnailPaths { get; set; }

        public PhotosModel()
        {
            OutpurDir = "C:\\Users\\Liora\\Documents\\OutputDir\\Thumbnails";
            ThumbnailPaths = Directory.GetFiles(OutpurDir, "*.thumb", SearchOption.AllDirectories);
        }
        
    }
}