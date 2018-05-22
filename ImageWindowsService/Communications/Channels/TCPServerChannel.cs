using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Infrastructure.Event;
using ImageService.Infrastructure.Enums;

namespace Communications.Channels
{
    public class TCPServerChannel
    {
        //private CommandEventArgs c = new ImageService.Infrastructure.Event.CommandEventArgs();
        private bool stop;
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        public event EventHandler<CommandEventArgs> OnMessageToServer;
        private Mutex mutex;
        private IClientHandler clientHandler;
        //private NetworkStream stream;
        //manual reset event

        public TCPServerChannel(IClientHandler handler)
        {
            /* IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
             listener = new TcpListener(ep);*/
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
            clientHandler = handler;
            mutex = new Mutex();
            stop = false;
        }

        public void Start()
        {
            Task.Run(() => { 
            listener.Start();
            //serverChannel = new TCPServerChannel(listener);
            Console.WriteLine("Waiting for client connections...");

            while (!stop)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
               // client.Client.Blocking = false;
                clients.Add(client);
                Task.Run(() => clientHandler.HandleClient(client));
                //Task.Run(() => ReceiveMessageFromClient(client));
                //logger.Log("Client" + client.ToString() + "is connected", MessageTypeEnum.INFO);
                //serverChannel.SendMessageToClient();
            }
            });
        }

        //make sure to have listener.accept turned on already in imageserver, or here?
        public void AcceptClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                while (!stop)
                {
                    if (stream.DataAvailable)
                    {
                        string message = reader.ReadString();
                        CommandEventArgs commandArgs = new CommandEventArgs(); //make sure tanslate from jason
                        OnMessageToServer?.Invoke(this, commandArgs);
                    }
                    else { Thread.Sleep(10); }
                }
            }
        }

        public void ReceiveMessageFromClient(TcpClient client)
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
                            string jsonString = reader.ReadString();
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                            //CommandEventArgs commandArgs = new CommandEventArgs(); //make sure tanslate from jason
                            Task.Run(() => OnMessageToServer?.Invoke(client, (CommandEventArgs)obj));
                        }
                        catch { Thread.Sleep(1); }
                    }
                }
                catch
                {
                    break;//stop = true;

                }

            }
        }

        public void SendMessageToClient(TcpClient client, CommandEventArgs commandArgs)
        {
            //NetworkStream stream = client.GetStream();
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(commandArgs);
                //string message = ""; //make it in jason
                mutex.WaitOne();
                writer.Write(jsonString);
                mutex.ReleaseMutex();
                //Dont forget try/catch
            }
        }

        public void SendMessageToAllClients(CommandEventArgs commandArgs)
        {
            for (int i = clients.Count - 1; i >= 0; i--)
            {
                if (!clients[i].Connected) clients.RemoveAt(i);
            }

            List<TcpClient> tempClients = clients;
            foreach (TcpClient client in tempClients)
            {
                //NetworkStream stream = client.GetStream();
                using (NetworkStream stream = client.GetStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    try
                    {
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(commandArgs);
                        //string message = ""; //make it in jason
                        mutex.WaitOne();
                        writer.Write(jsonString);
                        mutex.ReleaseMutex();
                        //Dont forget try/catch
                    }
                    catch { }
                }
            }
        }

        public void Stop()
        {
            stop = true;
            listener.Stop();
        }
    }
}
