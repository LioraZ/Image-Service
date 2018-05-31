using ImageService.Commands;
using ImageService.Model;
using Infrastructure.WebAppInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    class NumImagesCommand : ICommand
    {
        private IImageServiceModel model;

        public NumImagesCommand(IImageServiceModel imageModel)
        {
            model = imageModel;
        }
        public string Execute(string[] args, out bool result)
        {
            return model.GetNumImages(out result).ToString();
        }
    }
}
