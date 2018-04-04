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
        private Dictionary<int, CommandRecievedEventArgs>
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer()
        {
            IImageServiceModel imageModel = new ImageServiceModel();
        }
        public void CreateHandler(string directory)
        {
            Handler h = new Handler(directory, controller);
            CommandRecieved += h.onCommandReceived();
            h.onClose += onCloseServer;
        }
        public void CloseHandler() { }
        public void SendCommand()
        {
            onCommandReceived("*", CloseHandler);
        }
        public void OnCloseServer(object sender)
        {
            h = sender;
            onCommand -= HashSet.OnCommandReceived;
            OnCommand -= HashSet.OnCloseSErver;
        }
    }
}
