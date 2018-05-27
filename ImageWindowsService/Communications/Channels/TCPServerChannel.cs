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
    public class TCPServerChannel
    {
        private bool stop;
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        public event EventHandler<CommandEventArgs> OnMessageToServer;
        private Mutex mutex;
        private IClientHandler clientHandler;

        public TCPServerChannel(IClientHandler handler)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
            clientHandler = handler;
            mutex = handler.UpdateMutex();
            clientHandler.ClientDisconnect += OnClientDisconnect;
            stop = false;
        }

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

        private void OnClientDisconnect(object sender, TcpClient client)
        {
            mutex.WaitOne();
            clients.Remove(client);
            mutex.ReleaseMutex();
        }
        /*public void ReceiveMessageFromClient(TcpClient client)
        {
            while (!stop)
            {
                try
                {
                    using (NetworkStream stream = client.GetStream())
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        try
                        {
                            mutex.WaitOne();
                            string jsonString = reader.ReadString();
                            mutex.ReleaseMutex();
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                            Task.Run(() => OnMessageToServer?.Invoke(client, (CommandEventArgs)obj));
                        }
                        catch { Thread.Sleep(1); }
                    }
                }
                catch { break; }
            }
        }*/

        public void SendMessageToClient(TcpClient client, CommandEventArgs commandArgs)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(commandArgs);
           // mutex.WaitOne();
            writer.Write(jsonString);
            //mutex.ReleaseMutex();
        }

        public void SendMessageToAllClients(CommandEventArgs commandArgs)
        {
            /*for (int i = clients.Count - 1; i >= 0; i--)
            {
                try
                {
                    BinaryWriter writer = new BinaryWriter(clients[i].GetStream());
                    writer.Write("");
                }
                catch {
                    //clients[i].Close();
                    clients.RemoveAt(i);
                }
            }*/

            List<TcpClient> tempClients = clients;
            foreach (TcpClient client in tempClients)
            {
                SendMessageToClient(client, commandArgs);
            }
        }

        public void Stop()
        {
            stop = true;
            listener.Stop();
        }
    }
}
