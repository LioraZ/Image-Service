using ImageService.Model;
using System;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        /// <summary>
        /// The Event notifies when the directory it's watching is being used.
        /// </summary>
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>
        /// The method calls to start watching the directory.
        /// </summary>
        /// <param name="dirPath"></param> THe directory's path.
        void StartHandleDirectory(string dirPath);

        /// <summary>
        /// The event that will be activated upon receiving a new command.
        /// </summary>
        /// <param name="sender"></param> The event invoker.
        /// <param name="e"></param> The event's parameters.
        void OnCommandRecieved(object sender, CommandReceivedEventArgs e);
    }
}
