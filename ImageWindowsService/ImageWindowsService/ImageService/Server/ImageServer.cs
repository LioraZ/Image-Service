﻿using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
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

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController controller;        // The Command Controller
        private ILoggingService logger;             // The Image Service Event Logger
        private Dictionary<string, int> commands;   // The Commands Dictionary
        #endregion
        private TcpListener listener;
        private bool stop = false;

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
            commands = new Dictionary<string, int>()
            {
                {"Close Handler", (int)CommandEnum.CloseCommand },
                {"GetConfigCommand", (int)CommandEnum.GetConfigCommand }
            };
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
        }

        public void Start()
        {///do this in task
            
            listener.Start();
            Console.WriteLine("Waiting for client connections...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected");
            logger.Log("Client" + client.ToString() + "is connected", MessageTypeEnum.INFO);
            while (!stop)
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    Debug.WriteLine("Waiting for a message...");
                    //string command = reader.ReadString();
                    //Debug.WriteLine(command);
                    bool result = false;
                    string commandResult = "";
                    try
                    {
                        int commandID = reader.ReadInt32();
                        //int commandID = commands[command];
                        commandResult = controller.ExecuteCommand(commandID, null, out result);
                    }
                    catch { }
                    writer.Write(commandResult);
                    //SendCommand(command, "", null);
                    //writer.Write(num);
                }
            }
            
            client.Close();
            Stop();
        }

        public void Stop()
        {
            listener.Stop();
        }

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
                Stop();
            }
            catch
            {
                logger.Log("Couldn't cast the sender to handler", MessageTypeEnum.FAIL);
            }
        }
    }
}
