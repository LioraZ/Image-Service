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

        public event EventHandler<CommandEventArgs> OnDataReceived;

        public WebClient()
        {
            client = new TCPClientChannel();
            client.OnMessageFromServer += Client_OnMessageFromServer;
        }

        private void Client_OnMessageFromServer(object sender, CommandEventArgs e)
        {
            OnDataReceived?.Invoke(this, e);
        }

        public void SendCommand(CommandEnum commandID)
        {
            client.SendMessageToServer(new CommandEventArgs() { CommandID = commandID, CommandArgs = new string[] { } } );
            //client.
            return;
        }

        public bool Connect()
        {
            return client.ConnectToServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000));
        }
    }
}