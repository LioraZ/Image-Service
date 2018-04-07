using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel model;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModel imageModel)
        {
            model = imageModel;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
				// For Now will contain NEW_FILE_COMMAND
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand command = commands[commandID];
           // Write Code Here
        }
    }
}
