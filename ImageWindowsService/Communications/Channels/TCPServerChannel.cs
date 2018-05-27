using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Infrastructure.Event;

namespace Communications.Channels
{
    /// <summary>
    /// Class TCPServerChannel.
    /// </summary>
    public class TCPServerChannel
    {
        private bool stop;
        /// <summary>
        /// The listener
        /// </summary>
        private TcpListener listener;
        /// <summary>
        /// The clients
        /// </summary>
        private List<TcpClient> clients = new List<TcpClient>();
        public event EventHandler<CommandEventArgs> OnMessageToServer;
        /// <summary>
        /// The mutex
        /// </summary>
        private Mutex mutex;
        /// <summary>
        /// The client handler
        /// </summary>
        private IClientHandler clientHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TCPServerChannel"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public TCPServerChannel(IClientHandler handler)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
            clientHandler = handler;
            mutex = handler.UpdateMutex();
            clientHandler.ClientDisconnect += OnClientDisconnect;
            stop = false;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Task.Run(() => {
                listener.Start();
                Console.WriteLine("Waiting for client connections...");

                while (!stop)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client connected");
                    clients.Add(client);
                    Task.Run(() => clientHandler.HandleClient(client));
                }
            });
        }

        /// <summary>
        /// Called when [client disconnect].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="client">The client.</param>
        private void OnClientDisconnect(object sender, TcpClient client)
        {
            mutex.WaitOne();
            clients.Remove(client);
            mutex.ReleaseMutex();
        }
        public void SendMessageToClient(TcpClient client, CommandEventArgs commandArgs)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(commandArgs);
            mutex.WaitOne();
            writer.Write(jsonString);
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Sends the message to all clients.
        /// </summary>
        /// <param name="commandArgs">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        public void SendMessageToAllClients(CommandEventArgs commandArgs)
        {
            foreach (TcpClient client in clients)
            {
                SendMessageToClient(client, commandArgs);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            stop = true;
            listener.Stop();
        }
    }
}
