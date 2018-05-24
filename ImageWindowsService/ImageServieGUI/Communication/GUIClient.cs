using Communications.Channels;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServieGUI.Communication
{
    public class GUIClient
    {
        private static GUIClient instance = null;
        private TCPClientChannel clientChannel;
        public bool isConnected;
        public event EventHandler<CommandEventArgs> MessageReceived;
        public event EventHandler<bool> CheckConnection;

        public static GUIClient GetInstance()
        {
            if (instance == null) instance = new GUIClient();
            return instance;
        }

        private GUIClient()
        {
            clientChannel = new TCPClientChannel();
            clientChannel.OnMessageFromServer += ReceiveMessageFromServer;
            clientChannel.DisconnectedFromServer += OnDisconnectFromServer;
            isConnected = clientChannel.ConnectToServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000));
            CheckConnection?.Invoke(this, isConnected);
        }

        /**public bool ConnectToServer()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            try
            {
                client.Connect(ep);
                isConnected = true;
                Console.WriteLine("You are connected");
                return true;
            }
            catch
            {
                Console.WriteLine("Unable to connect to server. Please check your connection.");
                isConnected = false;
                return false;
            }
        }*/

        public void SendMessageToServer(CommandEnum commandID, string[] args)
        {
            Task.Run(()=>clientChannel.SendMessageToServer(new CommandEventArgs() { CommandID = commandID, CommandArgs = args }));
        }

        public void ReceiveMessageFromServer(object sender, CommandEventArgs args)
        {
            MessageReceived?.Invoke(this, args);
        }

        public void OnDisconnectFromServer(object sender, bool connected)
        {
            isConnected = connected;
            CheckConnection?.Invoke(this, isConnected);
        }
        
        //client channel should do this
        public void DisconnectFromServer()
        {
            //client.Close();
        }
    }
}
