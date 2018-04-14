using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        #region Members
        private IImageServiceModel model;                      // The Image Model
        private Dictionary<int, ICommand> commands;            // The Commads Dictionary
        #endregion

        /// <summary>
        /// The ImageController constructor.
        /// </summary>
        /// <param name="imageModel"></param> The image model.
        public ImageController(IImageServiceModel imageModel)
        {
            model = imageModel;
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(model)}
            };
        }

        /// <summary>
        /// The method will call the right command given the command ID, to be executed.
        /// </summary>
        /// <param name="commandID"></param> The given command ID.
        /// <param name="args"></param> THe command's args.
        /// <param name="resultSuccessful"></param> The result of the command, success/failure.
        /// <returns></returns>
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
