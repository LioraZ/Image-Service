using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController controller;        // The Command Controller
        private ILoggingService logger;             // The Image Service Event Logger
        private Dictionary<string, int> commands;   // The Commands Dictionary
        #endregion

        #region Properties
        public event EventHandler<CommandReceivedEventArgs> CommandReceived;          // The event that notifies about a new Command being recieved
        #endregion

        /// <summary>
        /// The ImageServer's constructor.
        /// </summary>
        /// <param name="imageLogger">The image logger</param>
        /// <param name="imageController">The image controller</param>
        public ImageServer(ILoggingService imageLogger, IImageController imageController)
        {
            controller = imageController;
            logger = imageLogger;
            commands = new Dictionary<string, int>()
            {
                {"Close Handler", (int)CommandEnum.CloseCommand }
            };
        }

        /// <summary>
        /// The method creates the directory handlers and strts them.
        /// </summary>
        /// <param name="directory">The directory path</param>
        public void CreateHandler(string directory)
        {
            if (!Directory.Exists(directory)) return;
            IDirectoryHandler h = new DirectoryHandler(directory, controller, logger);
            CommandReceived += h.OnCommandRecieved;
            h.DirectoryClose += OnCloseServer;
            h.StartHandleDirectory(directory);
        }

        /// <summary>
        /// The method invokes the command received event.
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="path">The file's path</param>
        /// <param name="args">The command's args</param>
        public void SendCommand(string command, string path, string[] args)
        {
            CommandReceived?.Invoke(this, new CommandReceivedEventArgs(commands[command], args, path));
        }

        /// <summary>
        /// The method is invoked by the closeDirectory event, to remove the handler from the server.
        /// </summary>
        /// <param name="sender">The sender-handler</param>
        /// <param name="args">The args</param>
        public void OnCloseServer(object sender, DirectoryCloseEventArgs args)
        {
            try
            {
                IDirectoryHandler h = (DirectoryHandler)sender;
                CommandReceived -= h.OnCommandRecieved;
                h.DirectoryClose -= OnCloseServer;
            }
            catch
            {
                logger.Log("Couldn't cast the sender to handler", MessageTypeEnum.FAIL);
            }
        }
    }
}
