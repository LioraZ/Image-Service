using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models.PhotosModel
{
    public class PhotoInfo
    {
        [DataType(DataType.Text)]
        [Display(Name = "Full Path")]
        public string FullPath { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Relative Path")]
        public string RelativePath { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Path")]
        public string ThumbnailPath { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Relative Thumbnail Path")]
        public string RelativeThumbnailPath { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Date")]
        public string Date { get; set; }
    }
}