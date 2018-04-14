using System;

namespace ImageService.Model
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }       // The Directory Path
        public string Message { get; set; }             // The Logged Message

        /// <summary>
        /// The DirectoryCloseEventArgs constructor.
        /// </summary>
        /// <param name="dirPath"></param> The directory path.
        /// <param name="message"></param> The message to be logged.
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;
            Message = message;
        }

    }
}
