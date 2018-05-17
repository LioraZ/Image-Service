using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class TCPServerChannel
    {
        private TcpListener listener;
        public event EventHandler<CommandEventArgs> OnMessageToServer;

        public TCPServerChannel(TcpListener tcpListener)
        {
            /* IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
             listener = new TcpListener(ep);*/
            listener = tcpListener;
        }

        //make sure to have listener.accept turned on already in imageserver, or here?
        public void AcceptClient()
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected");
            //logger.Log("Client" + client.ToString() + "is connected", MessageTypeEnum.INFO);
            Task.Run(() => ReceiveMessageFromClient(client));
        }

        public void ReceiveMessageFromClient(TcpClient client)
        {
            //either server can send message, or client can
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                Debug.WriteLine("Waiting for a message...");
                try
                {
                    string message = reader.ReadString();
                    CommandEventArgs commandArgs = new CommandEventArgs(); //make sure tanslate from jason
                    OnMessageToServer?.Invoke(this, commandArgs);
                }
                catch { }
            }
        }

        public void SendMessageToClient(CommandEventArgs commandArgs)
        {
            TcpClient client; //make sure to initialzie
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                string message = ""; //make it in jason
                writer.Write(message);
                //Dont forget try/catch
            }
        }

    }
}
