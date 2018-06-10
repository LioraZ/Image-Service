using Communications.Channels;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ImageServiceWeb.Models.Communication
{


    public class WebClient : IWebClient
    {
        private TCPClientChannel client;

        private static WebClient instance;

        public bool Connected { get; set; }

        public event EventHandler<CommandEventArgs> OnDataReceived;

        public static WebClient GetInstance()
        {
            if (instance == null) instance = new WebClient();
            return instance;
        }

        private WebClient()
        {
            client = new TCPClientChannel();
            client.OnMessageFromServer += Client_OnMessageFromServer;
            Connect();
        }

        private void Client_OnMessageFromServer(object sender, CommandEventArgs e)
        {
            OnDataReceived?.Invoke(this, e);
        }

        public void SendCommand(CommandEnum commandID)
        {
            SendCommand(commandID, new string[] { });
        }

        public bool Connect()
        {
             Connected = client.ConnectToServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000));
             return Connected;
        }

        public bool isConnected()
        {
            return Connected;
        }

        public void SendCommand(CommandEnum commandID, string[] args)
        {
            client.SendMessageToServer(new CommandEventArgs() { CommandID = commandID, CommandArgs = args });
        }
    }
}