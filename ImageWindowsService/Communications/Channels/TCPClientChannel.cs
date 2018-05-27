using ImageService.Infrastructure.Event;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Communications.Channels
{
    /// <summary>
    /// Class ReceiveMessageFromServer.
    /// </summary>
    public class TCPClientChannel
    {
        public event EventHandler<bool> DisconnectedFromServer;
        public event EventHandler<CommandEventArgs> OnMessageFromServer;
        protected TcpClient client;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TCPClientChannel"/> is stop.
        /// </summary>
        /// <value><c>true</c> if stop; otherwise, <c>false</c>.</value>
        public bool Stop { get; set; }
        /// <summary>
        /// The mutex
        /// </summary>
        protected static Mutex mutex;

        /// <summary>
        /// Initializes a new instance of the <see cref="TCPClientChannel"/> class.
        /// </summary>
        public TCPClientChannel()
        {
            client = new TcpClient();
            Stop = false;
            mutex = new Mutex();
        }

        /// <summary>
        /// Connects to server.
        /// </summary>
        /// <param name="ep">The ep.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ConnectToServer(IPEndPoint ep)
        {
            try
            {
                client.Connect(ep);
                Console.WriteLine("You are connected");
                Task.Run(() => ReceiveMessageFromServer());
                Stop = false;
                return true;
            }
            catch
            {
                Console.WriteLine("Unable to connect to server. Please check your connection.");
                return false;
            }
        }

        /// <summary>
        /// Sends the message to server.
        /// </summary>
        /// <param name="commandArgs">The <see cref="CommandEventArgs" /> instance containing the event data.</param>
        public void SendMessageToServer(CommandEventArgs commandArgs)
        {
            if (!client.Connected)
            {
                DisconnectFromServer();
                //DisconnectedFromServer?.Invoke(this, client.Connected);
                //Stop = true;
                return;
            }
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);
            {
                try
                {
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(commandArgs);
                    mutex.WaitOne();
                    writer.Write(jsonString);
                    jsonString = reader.ReadString();
                    mutex.ReleaseMutex();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                    if (obj != null)
                    {
                        OnMessageFromServer?.Invoke(this, (CommandEventArgs)obj);
                    }

                }
                catch { Debug.WriteLine("Can't write to server"); }
            }
        }

        /// <summary>
        /// Receives the message from server.
        /// </summary>
        public void ReceiveMessageFromServer()
        {
            while (!Stop)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    try
                    {
                        if (stream.DataAvailable)
                        {
                            mutex.WaitOne();
                            string jsonString = reader.ReadString();
                            mutex.ReleaseMutex();
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                            if (obj != null)
                            {
                                OnMessageFromServer?.Invoke(this, (CommandEventArgs)obj);
                            }
                        }
                        else { Thread.Sleep(2); }
                    }
                    catch { DisconnectFromServer(); }
                }
                catch { DisconnectFromServer(); }
            }
        }

        /// <summary>
        /// Disconnects from server.
        /// </summary>
        public void DisconnectFromServer()
        {
            Stop = true;
            //  client.Close();
            DisconnectedFromServer?.Invoke(this, false);
        }
    }
}
