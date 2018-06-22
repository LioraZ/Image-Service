using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Model.Event
{
    public class ImageData
    {
        public string Name { get; set; }
        public Byte[] ImageBytes { get; set; }
    }
}
