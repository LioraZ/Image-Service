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
    public class TCPClientChannel
    {
        public event EventHandler<bool> DisconnectedFromServer;
        public event EventHandler<CommandEventArgs> OnMessageFromServer;
        protected TcpClient client;
        public bool Stop { get; set; }
        protected static Mutex mutex;

        public TCPClientChannel()
        {
            client = new TcpClient();
            Stop = false;
            mutex = new Mutex();
        }

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

        public void SendMessageToServer(CommandEventArgs commandArgs)
        {
            if (!client.Connected)
            {
                DisconnectedFromServer?.Invoke(this, client.Connected);
                Stop = true;
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
                    mutex.ReleaseMutex();
                }
                catch { Debug.WriteLine("Can't write to server"); }
            }
        }

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
                    catch { Stop = true; }
                }
                catch { Stop = true; }
            }      
        }

        public void DisconnectFromServer()
        {
            Stop = true;
            DisconnectedFromServer?.Invoke(this, true);
        }
    }
}
