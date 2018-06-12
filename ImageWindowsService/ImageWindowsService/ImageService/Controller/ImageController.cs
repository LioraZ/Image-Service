using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using ImageWindowsService.ImageService.Commands;
using System.Collections.Generic;
using System;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        #region Members
        private IImageServiceModel model;                      // The Image Model
        private Dictionary<CommandEnum, ICommand> commands;            // The Commads Dictionary
        #endregion

        /// <summary>
        /// The ImageController constructor.
        /// </summary>
        /// <param name="imageModel"></param> The image model.
        public ImageController(IImageServiceModel imageModel)
        {
            model = imageModel;
            commands = new Dictionary<CommandEnum, ICommand>()
            {
                {CommandEnum.NewFileCommand, new NewFileCommand(model)},
                {CommandEnum.GetConfigCommand, new GetCongigCommand() },
                {CommandEnum.RemoveHandlerCommand, new RemoveHandlerCommand() },
                {CommandEnum.GetAllLogsCommand, new GetAllLogsCommand() },
                {CommandEnum.GetStudentsInfo, new GetStudentsInfoCommand() },
                {CommandEnum.GetNumImages, new NumImagesCommand(model) }
              //  {CommandEnum.LogCommand, new LogCommand() }
            };
        }

        /// <summary>
        /// The method will call the right command given the command ID, to be executed.
        /// </summary>
        /// <param name="commandID"></param> The given command ID.
        /// <param name="args"></param> THe command's args.
        /// <param name="resultSuccessful"></param> The result of the command, success/failure.
        /// <returns></returns>
        public string ExecuteCommand(CommandEnum commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.ContainsKey(commandID))
            {
                string message = commands[commandID].Execute(args, out resultSuccesful);
                return message;
            }
            resultSuccesful = false;
            return "Wrong command!";
        }

        public void AddCommand(CommandEnum commandID, ICommand command)
        {
            if (commands.ContainsKey(commandID)) commands[commandID] = command;
            else commands.Add(commandID, command);
        }
    }
}
