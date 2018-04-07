using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
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
        private Dictionary<string, CommandReceivedEventArgs> commands;
        #endregion

        #region Properties
        public event EventHandler<CommandReceivedEventArgs> CommandReceived;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(ILoggingService imageLogger)
        {
            IImageServiceModel imageModel = new ImageServiceModel();
            controller = new ImageController(imageModel);
            logger = imageLogger;
            commands = new Dictionary<string, CommandReceivedEventArgs>();
        }

        private void AddCommands()
        {
            commands.Add("close handler", );
        }

        public void CreateHandler(string directory)
        {
            IDirectoryHandler h = new DirectoryHandler(directory, controller);
            CommandReceived += h.OnCommandRecieved;
            h.DirectoryClose += OnCloseServer;
        }
        public void CloseHandler() { }
        public void SendCommand(string command)
        {
            //CommandReceived(command, CloseHandler);
            CommandReceived(this, commands[command]);
        }
        public void OnCloseServer(object sender, DirectoryCloseEventArgs args)
        {
            IDirectoryHandler h = (DirectoryHandler)sender; //check that is legal
            CommandReceived -= h.OnCommandRecieved;
            //h.DirectoryClose -= OnCloseServer;
        }
    }
}
