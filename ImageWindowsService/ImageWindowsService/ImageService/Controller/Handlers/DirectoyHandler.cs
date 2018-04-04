using ImageService.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController controller;              // The Image Processing Controller
        private ILoggingService logger;
        private FileSystemWatcher dirWatcher;             // The Watcher of the Dir
        private string path;                              // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

		// Implement Here!
        public void StartHandleDirectory(string dirPath) { }
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e) { }
    }
}
