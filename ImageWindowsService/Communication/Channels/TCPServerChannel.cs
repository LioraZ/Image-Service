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

namespace Communication
{
    class TCPServerChannel
    {
        private bool stop;
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        public event EventHandler<CommandEventArgs> OnMessageToServer;
        //manual reset event

        public TCPServerChannel(TcpListener tcpListener)
        {
            /* IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
             listener = new TcpListener(ep);*/
            listener = tcpListener;
            stop = false;
        }

        public void Start()
        {
            listener.Start();
            //serverChannel = new TCPServerChannel(listener);
            Console.WriteLine("Waiting for client connections...");

            while (!stop)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                clients.Add(client);
                Task.Run(() => ReceiveMessageFromClient(client));
                //logger.Log("Client" + client.ToString() + "is connected", MessageTypeEnum.INFO);
                //serverChannel.SendMessageToClient();
                //Task.Run(() => OpenCommunicationStream(client));
            }
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
                    
                /**try
                {
                    // ShutdownEvent is a ManualResetEvent signaled by
                    // Client when its time to close the socket.
                    while (!ShutdownEvent.WaitOne(0))
                    {
                        try
                        {
                            // We could use the ReadTimeout property and let Read()
                            // block.  However, if no data is received prior to the
                            // timeout period expiring, an IOException occurs.
                            // While this can be handled, it leads to problems when
                            // debugging if we are wanting to break when exceptions
                            // are thrown (unless we explicitly ignore IOException,
                            // which I always forget to do).
                            if (stream.DataAvailable)
                            {
                                // Give up the remaining time slice.
                                Thread.Sleep(1);
                            }
                            else if (stream.Read(_data, 0, _data.Length) > 0)
                            {
                                // Raise the DataReceived event w/ data...
                            }
                            else
                            {
                                // The connection has closed gracefully, so stop the
                                // thread.
                                ShutdownEvent.Set();
                            }
                        }
                        catch (IOException ex)
                        {
                            // Handle the exception...
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception...
                }**/
            //Task.Run(() => ReceiveMessageFromClient(client));
        }

        public void ReceiveMessageFromClient(TcpClient client)
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
                        OnMessageToServer?.Invoke(client, commandArgs);
                    }
                    else { Thread.Sleep(10); }
                }
            }
        }

        public void SendMessageToClient(TcpClient client, CommandEventArgs commandArgs)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                string message = ""; //make it in jason
                writer.Write(message);
                //Dont forget try/catch
            }
        }

        public void SendMessageToAllClients(CommandEventArgs commandArgs)
        {
            foreach (TcpClient client in clients)
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    if (!client.Connected)
                    {
                        clients.Remove(client);
                        continue;
                    }
                    string message = ""; //make it in jason
                    writer.Write(message);
                    //Dont forget try/catch
                }
            }
        }

    }
}
