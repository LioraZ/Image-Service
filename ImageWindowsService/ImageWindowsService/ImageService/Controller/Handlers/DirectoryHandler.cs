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

        public DirectoryHandler(string path, IImageController imageController)
        {
            controller = imageController;
            dirPath = path;
            dirWatcher = new FileSystemWatcher(dirPath, "*.gif|jpe?g|bmp|png");
            //check notifiers
            dirWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.OnChanged;
        }
        public void StartHandleDirectory(string dirPath)
        {
            dirWatcher.EnableRaisingEvents = true;
            dirWatcher.Created += new FileSystemEventHandler(OnChanged);
        }
        public void OnCommandRecieved(object sender, CommandReceivedEventArgs e)
        {
            bool result;
            string imgPath = e.RequestDirPath;
            //if (imgPath) file is valid
            if (!IsSubFile(imgPath)) return;
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                CloseHandler();
                return;
            }
            string msg = controller.ExecuteCommand(e.CommandID, e.Args, out result);
            logger.Log(msg, GetMessageType(result));
            //DirectoryClose?.Invoke(this, )
        }
        private bool IsSubFile(string file)
        {
            while (Directory.GetParent(file).Name != dirPath)
            {
                file = Directory.GetParent(file).Name;
            }
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
        private void CallCommand(object source, FileSystemEventArgs e)
        {

        }

        public static DateTime DateTaken(string path)
        {
            Regex r = new Regex(":");
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            DateTime date = DateTaken(e.Name);
            
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
    }
}
