using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class TCPClientChannel
    {
        public event EventHandler<CommandEventArgs> OnMessageFromServer;
        private TcpClient client;

        public TCPClientChannel(IPEndPoint ep)
        {
            //take care of servver disconnected event in Gui client
            ConnectToServer(ep);
        }

        public bool ConnectToServer(IPEndPoint ep)
        {
            client = new TcpClient();
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

        public void SendMessageToServer(TcpClient client, CommandEnum commandID, string[] args)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                try
                {
                    CommandEventArgs commandArgs = new CommandEventArgs() { CommandID = commandID, CommandArgs = args };
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
                //Dont forget try/catch
                string message = reader.ReadString();
                //change string through jason to command evnt args
                CommandEventArgs commandArgs = new CommandEventArgs(); //make sure to jasonize!
                Task.Run(() => { OnMessageFromServer?.Invoke(this, commandArgs); });//call the CLient class to invoke event.maybe cand send a task?
               
            }
        }
    }
}
