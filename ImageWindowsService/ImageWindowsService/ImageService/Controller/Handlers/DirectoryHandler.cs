using ImageService.Model;
using System;
using System.IO;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        #region Members
        private IImageController controller;              // The Image Processing Controller
        private ILoggingService logger;                   // The Image Service Logger
        private FileSystemWatcher dirWatcher;             // The Watcher of the Directory
        private string dirPath;                           // The Path of directory
        #endregion

        /// <summary>
        /// The event notifies the director that it's being closed.
        /// </summary>
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>
        /// The DirectoryHandler's constructor.
        /// </summary>
        /// <param name="path"></param> The path to watch.
        /// <param name="imageController"></param> The commands controller.
        /// <param name="imageLogger"></param> The image service logger.
        public DirectoryHandler(string path, IImageController imageController, ILoggingService imageLogger)
        {
            controller = imageController;
            dirPath = path;
            logger = imageLogger;
            dirWatcher = new FileSystemWatcher(dirPath);
        }

        /// <summary>
        /// The method starts watching the directory.
        /// </summary>
        /// <param name="dirPath"></param> The directory's path.
        public void StartHandleDirectory(string dirPath)
        {
            dirWatcher.EnableRaisingEvents = true;
            dirWatcher.Created += new FileSystemEventHandler(CallNewFileCommand);
            logger.Log("Watching " + dirPath, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// The event occur upon receiving a command.
        /// </summary>
        /// <param name="sender"></param> The command sender (invoker).
        /// <param name="e"></param> The argumetns.
        public void OnCommandRecieved(object sender, CommandReceivedEventArgs e)
        {
            string msg;
            bool result = true;
            if (e.CommandID == CommandEnum.CloseCommand)
            {
                if (e.RequestDirPath != dirPath) return;
                msg = "Directory " + dirPath + " is closing";
                dirWatcher.EnableRaisingEvents = false;
                DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(e.RequestDirPath, msg));
                //logger.Log(msg, MessageTypeEnum.INFO);
            }
            else
            {
                if (!IsSubFile(e.RequestDirPath)) return;
                msg = controller.ExecuteCommand(e.CommandID, e.Args, out result);
            }
            logger.Log(msg, GetMessageType(result));
        }

        /// <summary>
        /// The method checks if the given path is contained in the current wached directory.
        /// </summary>
        /// <param name="file"></param> The given path.
        /// <returns></returns> True if the path is in the current directory, and else false.
        private bool IsSubFile(string file)
        {
            //if (file == dirPath) return true;
            while (Directory.GetParent(file).Name != dirPath) file = Directory.GetParent(file).Name;
            return file != "";
        }

        /// <summary>
        /// The method returns the Messgae Type given a boolean value.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>MessageTypeEnum.</returns>
        /// The given boolean value.
        /// INFO or FAIL based on boolen value.
        private MessageTypeEnum GetMessageType(bool result)
        {
            if (result) return MessageTypeEnum.INFO;
            return MessageTypeEnum.FAIL;
        }

        /// <summary>
        /// The method calls the new file command when invoked by the watcher.
        /// </summary>
        /// <param name="source"></param> The invoker.
        /// <param name="e"></param> The arguments.
        private void CallNewFileCommand(object source, FileSystemEventArgs e)
        {
            bool result;
            string[] args = { e.FullPath };
            if (FilterExtension(e.FullPath))
            {
                string msg = controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
                logger.Log(msg, GetMessageType(result));
            }
            
        }

        /// <summary>
        /// The method checks if the path is a specified file type to be watched.
        /// </summary>
        /// <param name="path"></param> The path received after invoked by the watcher.
        /// <returns></returns> True if the path is the specified extension and else false.
        private bool FilterExtension(string path)
        {
            return (Regex.IsMatch(path, "^*\\.(jpe?g|png|bmp|gif)$", RegexOptions.IgnoreCase));
        }
    }
}
