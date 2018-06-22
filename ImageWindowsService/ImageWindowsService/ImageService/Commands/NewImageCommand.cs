using ImageService.Commands;
using ImageService.Model;
using ImageWindowsService.ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    public class NewImageCommand : ICommand
    {
        private IImageServiceModel model;

        public NewImageCommand(IImageServiceModel imageModel)
        {
            model = imageModel;
        }

        public string Execute(string[] args, out bool result)
        {
            new GetCongigCommand().Execute(args, out result);
            if (GetCongigCommand.handlers.Count == 0)
            {
                result = false;
                return "No directory handlers found to handle image";
            }
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageData>(args[0]);
            Byte[] imageBytes = obj.ImageBytes;
            string name = obj.Name;
            Image image = byteArrayToImage(imageBytes);
            string handler = GetCongigCommand.handlers[0];
            image.Save(handler + "\\" + name, System.Drawing.Imaging.ImageFormat.Jpeg);
            result = true;
            return "Image " + name + "successfully added to " + handler + "directory handler";
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Position = 0; // this is important
                return Image.FromStream(ms, true);
            }
            catch { return null; }
        }
    }
}
