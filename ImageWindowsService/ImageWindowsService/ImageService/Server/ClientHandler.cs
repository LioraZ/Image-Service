using Communications.Channels;
using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageService.Logging;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Server
{
    /// <summary>
    /// Class ClientHandler.
    /// </summary>
    /// <seealso cref="Communications.Channels.IClientHandler" />
    public class ClientHandler : IClientHandler
    {
        public event EventHandler<TcpClient> ClientDisconnect;
        /// <summary>
        /// The controller
        /// </summary>
        private IImageController controller;
        /// <summary>
        /// The logger
        /// </summary>
        private ILoggingService logger;
        /// <summary>
        /// The stop
        /// </summary>
        private bool stop;
        /// <summary>
        /// The mutex
        /// </summary>
        private static Mutex mutex;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHandler"/> class.
        /// </summary>
        /// <param name="imageController">The image controller.</param>
        /// <param name="imageLogger">The image logger.</param>
        public ClientHandler(IImageController imageController, ILoggingService imageLogger)
        {
            stop = false;
            mutex = new Mutex();
            controller = imageController;
            logger = imageLogger;
        }

        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void HandleClient(TcpClient client)
        {
            while (!stop)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    BinaryWriter writer = new BinaryWriter(stream);
                    {

                        try
                        {
                           // mutex.WaitOne();
                            string jsonString = reader.ReadString();
                           // mutex.ReleaseMutex();
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                            CommandEventArgs args = (CommandEventArgs)obj;
                            bool result;
                            string logMessage = controller.ExecuteCommand(args.CommandID, args.CommandArgs, out result); //make sure controller has try/catch
                            if (!result)
                            {
                                logger.Log(logMessage, MessageTypeEnum.FAIL);
                                return;
                            }
                            jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new CommandEventArgs() { CommandID = args.CommandID, CommandArgs = new string[] { logMessage } });
                            mutex.WaitOne();
                            if (args.CommandID != CommandEnum.RemoveHandlerCommand)
                            {
                                writer.Write(jsonString);
                                mutex.ReleaseMutex();
                                mutex.WaitOne();
                            }
                            logger.Log("Command " + args.CommandID.ToString() + " received" + System.Environment.NewLine + 
                               "Arguments returned:\n"  + logMessage, MessageTypeEnum.INFO);
                            mutex.ReleaseMutex();
                        }
                        catch
                        {
                            ClientDisconnect?.Invoke(this, client);
                            break;
                        }
                    }
                }
                catch
                {
                    ClientDisconnect?.Invoke(this, client);
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the mutex.
        /// </summary>
        /// <returns>Mutex.</returns>
        public Mutex UpdateMutex()
        {
            return mutex;
        }
    }
}
