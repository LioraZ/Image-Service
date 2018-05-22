using Communications.Channels;
using ImageService.Controller;
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
    class ClientHandler : IClientHandler
    {
        private IImageController controller;
        private ILoggingService logger;
        private bool stop;
        private static Mutex mutex;

        public ClientHandler(IImageController imageController, ILoggingService imageLogger)
        {
            stop = false;
            mutex = new Mutex();
            controller = imageController;
            logger = imageLogger;
        }

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
                            //mutex.WaitOne();
                            string jsonString = reader.ReadString();
                            //mutex.ReleaseMutex();
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                            CommandEventArgs args = (CommandEventArgs)obj; //make sure tanslate from jason
                            //Task.Run(() => OnMessageToServer?.Invoke(client, (CommandEventArgs)obj));
                            bool result;
                            string logMessage = controller.ExecuteCommand((int)args.CommandID, args.CommandArgs, out result); //make sure controller has try/catch
                            logger.Log(logMessage, MessageTypeEnum.INFO);
                            if (!result)
                            {
                                logger.Log(logMessage, MessageTypeEnum.FAIL);
                                return;
                            }

                            jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new CommandEventArgs() { CommandID = args.CommandID, CommandArgs = new string[] { logMessage } });
                            //string message = ""; //make it in jason
                            mutex.WaitOne();
                            writer.Write(jsonString);
                            mutex.ReleaseMutex();
                            //stop = true;
                            //serverChannel.SendMessageToClient(client, new CommandEventArgs() { CommandID = args.CommandID, CommandArgs = new string[] { logMessage } });
                            //serverChannel.SendMessageToClient(client, new CommandEventArgs() { CommandID = args.CommandID, CommandArgs = new string[] { logMessage } });
                        }
                        catch { Thread.Sleep(5); }
                    }
                }
                catch { }// stop = true; }
            }
        }
    }
}
