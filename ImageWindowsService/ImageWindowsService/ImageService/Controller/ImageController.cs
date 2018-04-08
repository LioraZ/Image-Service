using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel model;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModel imageModel)
        {
            model = imageModel;
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(model)}
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.ContainsKey(commandID))
            {
                return commands[commandID].Execute(args, out resultSuccesful);
            }
            resultSuccesful = false;
            return "Wrong command!";
        }
    }
}
