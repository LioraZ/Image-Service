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
    /// <summary>
    /// Class GUIClient.
    /// </summary>
    public class GUIClient
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static GUIClient instance = null;
        /// <summary>
        /// The client channel
        /// </summary>
        private TCPClientChannel clientChannel;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value><c>true</c> if this instance is connected; otherwise, <c>false</c>.</value>
        public static bool isConnected { get; set; }
        public event EventHandler<CommandEventArgs> MessageReceived;
        public event EventHandler<bool> CheckConnection;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>GUIClient.</returns>
        public static GUIClient GetInstance()
        {
            if (instance == null) instance = new GUIClient();
            return instance;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="GUIClient"/> class from being created.
        /// </summary>
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

        /// <summary>
        /// Sends the message to server.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        public void SendMessageToServer(CommandEnum commandID, string[] args)
        {
            Task.Run(()=>clientChannel.SendMessageToServer(new CommandEventArgs() { CommandID = commandID, CommandArgs = args }));
        }

        /// <summary>
        /// Receives the message from server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        public void ReceiveMessageFromServer(object sender, CommandEventArgs args)
        {
            MessageReceived?.Invoke(this, args);
        }

        /// <summary>
        /// Called when [disconnect from server].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="connected">if set to <c>true</c> [connected].</param>
        public void OnDisconnectFromServer(object sender, bool connected)
        {
            isConnected = connected;
            CheckConnection?.Invoke(this, isConnected);
        }

        //client channel should do this
        /// <summary>
        /// Disconnects from server.
        /// </summary>
        public void DisconnectFromServer()
        {
            //client.Close();
        }
    }
}
