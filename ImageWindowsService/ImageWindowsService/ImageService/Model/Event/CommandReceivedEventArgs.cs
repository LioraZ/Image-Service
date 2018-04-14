using System;

namespace ImageService.Model
{
    public class CommandReceivedEventArgs : EventArgs
    {
        public int CommandID { get; set; }           // The Command ID
        public string[] Args { get; set; }           // The Command Args
        public string RequestDirPath { get; set; }   // The Request Directory

        /// <summary>
        /// The CommandReceivedEventArgs constructor.
        /// </summary>
        /// <param name="id"></param> THe command ID.
        /// <param name="args"></param> The command args.
        /// <param name="path"></param> The request directory.
        public CommandReceivedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
