using ImageService.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        #region Members
        private IImageController controller;              // The Image Processing Controller
        private ILoggingService logger;
        private FileSystemWatcher dirWatcher;             // The Watcher of the Dir
        private string dirPath;                              // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoryHandler(string path, IImageController imageController, ILoggingService imageLogger)
        {
            controller = imageController;
            dirPath = path;
            logger = imageLogger;
            dirWatcher = new FileSystemWatcher(dirPath);
        }
        public void StartHandleDirectory(string dirPath)
        {
            dirWatcher.EnableRaisingEvents = true;
            dirWatcher.Created += new FileSystemEventHandler(CallNewFileCommand);
            logger.Log("Watching " + dirPath, MessageTypeEnum.INFO);
        }
        public void OnCommandRecieved(object sender, CommandReceivedEventArgs e)
        {
            string msg;
            bool result = true;
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                msg = "Directory " + dirPath + " is closing";
                dirWatcher.EnableRaisingEvents = false;
                DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(dirPath, msg));
            }
            else
            {
                if (!IsSubFile(e.RequestDirPath)) return;
                msg = controller.ExecuteCommand(e.CommandID, e.Args, out result);
            }
            logger.Log(msg, GetMessageType(result));
        }
        private bool IsSubFile(string file)
        {
            while (Directory.GetParent(file).Name != dirPath) file = Directory.GetParent(file).Name;
            return file != "";
        }
        private MessageTypeEnum GetMessageType(bool result)
        {
            if (result) return MessageTypeEnum.INFO;
            return MessageTypeEnum.FAIL;
        }
        public void CloseHandler()
        {
            //add try/catch
            dirWatcher.EnableRaisingEvents = false;
            //some code
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(dirPath, "Closing direcotry"));
        }
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
        private bool FilterExtension(string path)
        {
            return (Regex.IsMatch(path, "^*\\.(jpe?g|png|bmp|gif)$", RegexOptions.IgnoreCase));
        }
    }
}
