using Communications.Channels;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using ImageWindowsService.ImageService.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController controller;        // The Command Controller
        private ILoggingService logger;             // The Image Service Event Logger
        private Dictionary<string, CommandEnum> commands;   // The Commands Dictionary
        private TCPServerChannel serverChannel;
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
            logger.MessageReceived += OnLogMessageReceived;
            commands = new Dictionary<string, CommandEnum>()
            {
                {"Close Handler", CommandEnum.CloseCommand },
                {"GetConfigCommand", CommandEnum.GetConfigCommand }
            };
            ClientHandler clientHandler = new ClientHandler(controller, logger);
            serverChannel = new TCPServerChannel(clientHandler);
           // serverChannel.OnMessageToServer += OnMessageToServerReceived;
            serverChannel.Start(); //maybe outer task it instead of inner task
        }

        public void OnLogMessageReceived(object sender, MessageRecievedEventArgs args)
        {
            bool result;
            string message = controller.ExecuteCommand(CommandEnum.LogCommand, new string[] { args.Message, args.Status.ToString()}, out result);
            if (!result) return;//see if there is any way i can log message without causing loop.
            Task.Run(()=>serverChannel.SendMessageToAllClients(new CommandEventArgs() { CommandID = CommandEnum.LogCommand, CommandArgs = new string[] { args.Message, args.Status.ToString() } }));
            //Task.Run(() => OpenCommunicationStream(client));
        }

        public void OnRemoveHandler(object sender, string path)
        {
            
            SendCommand("Close Handler", path, new string[] { });
        }

        /*public void OnMessageToServerReceived(object sender, CommandEventArgs args)
        {
            bool result;
            TcpClient client = (TcpClient)sender;
            string logMessage = controller.ExecuteCommand((int)args.CommandID, args.CommandArgs, out result); //make sure controller has try/catch
            if (!result)
            {
                logger.Log(logMessage, MessageTypeEnum.FAIL);
                return;
            }
            Task.Run(()=>serverChannel.SendMessageToAllClients(new CommandEventArgs() { CommandID = args.CommandID, CommandArgs = new string[] { logMessage } }));
            //Task.Run(()=> serverChannel.SendMessageToClient(client, new CommandEventArgs() { CommandID = args.CommandID, CommandArgs = new string[] { logMessage } }));
        }*/

        /// <summary>
        /// The method creates the directory handlers and strts them.
        /// </summary>
        /// <param name="directory">The directory path</param>
        public void CreateHandler(string directory)
        {
            if (!Directory.Exists(directory))
            {
                try { Directory.CreateDirectory(directory); }
                catch { return; }
            }
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
                //serverChannel.Stop();
            }
            catch
            {
                logger.Log("Couldn't cast the sender to handler", MessageTypeEnum.FAIL);
            }
        }
    }
}
