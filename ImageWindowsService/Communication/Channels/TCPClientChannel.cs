using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    class TCPClientChannel
    {
        public event EventHandler<CommandEventArgs> OnMessageFromServer;
        private TcpClient client;
        private bool stop;

        public TCPClientChannel(IPEndPoint ep)
        {
            client = new TcpClient();
            stop = false;
            //take care of servver disconnected event in Gui client
            //ConnectToServer(ep);
        }

        public bool ConnectToServer(IPEndPoint ep)
        {
            try
            {
                client.Connect(ep);
               // isConnected = true;
                Console.WriteLine("You are connected");
                return true;
            }
            catch
            {
                Console.WriteLine("Unable to connect to server. Please check your connection.");
               // isConnected = false;
                return false;
            }
        }

        public void SendMessageToServer(CommandEventArgs commandArgss)
        {
            //check connection here first
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                try
                {
                    //CommandEventArgs commandArgs = new CommandEventArgs() { CommandID = commandID, CommandArgs = args };
                    string message = "";//jasonize commandargs
                    writer.Write(message);
                }
                catch { }
            }
        }

        public void ReceiveMessageFromServer()
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                while (!stop)
                {
                    //might need try catch if data.available not working
                    if (stream.DataAvailable)
                    {
                        string message = reader.ReadString();
                        CommandEventArgs commandArgs = new CommandEventArgs(); //make sure tanslate from jason
                        OnMessageFromServer?.Invoke(this, commandArgs);
                    }
                    else { Thread.Sleep(10); }
                }
            }
        }
    }
}
