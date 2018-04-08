﻿using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Model;
using ImageService.Logging;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController controller;
        private ILoggingService logger;
        private Dictionary<string, int> commands;
        #endregion

        #region Properties
        public event EventHandler<CommandReceivedEventArgs> CommandReceived;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(ILoggingService imageLogger)
        {
            IImageServiceModel imageModel = new ImageServiceModel();
            controller = new ImageController(imageModel);
            logger = imageLogger;
            commands = new Dictionary<string, int>();
        }

        private void AddCommands()
        {
            commands.Add("Close Handler", (int)CommandEnum.CloseCommand);
            commands.Add("Add File", (int)CommandEnum.CloseCommand);
        }

        public void CreateHandler(string directory)
        {
            IDirectoryHandler h = new DirectoryHandler(directory, controller);
            CommandReceived += h.OnCommandRecieved;
            h.DirectoryClose += OnCloseServer;
        }
        public void SendCommand(string command, string path, string[] args)
        {
            CommandReceived(this, new CommandReceivedEventArgs(commands[command], args, path));
        }
        public void OnCloseServer(object sender, DirectoryCloseEventArgs args)
        {
            //check to make sure casting is done right-might throw an exception
            IDirectoryHandler h = (DirectoryHandler)sender; //check that is legal
            CommandReceived -= h.OnCommandRecieved;
            h.DirectoryClose -= OnCloseServer;
            logger.Log(args.Message, MessageTypeEnum.INFO);//might need to put fail
        }
    }
}
