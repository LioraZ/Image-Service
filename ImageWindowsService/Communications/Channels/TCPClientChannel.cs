using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
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

namespace Communications.Channels
{
    public class TCPClientChannel
    {
        public event EventHandler<bool> DisconnectedFromServer;
        public event EventHandler<CommandEventArgs> OnMessageFromServer;
        protected TcpClient client;
        protected bool stop;
        protected static Mutex mutex;
       // private NetworkStream stream;

        public TCPClientChannel()
        {
            client = new TcpClient();
            stop = false;
            mutex = new Mutex();
            //take care of servver disconnected event in Gui client
            //ConnectToServer(ep);
        }

        public bool ConnectToServer(IPEndPoint ep)
        {
            try
            {
                client.Connect(ep);
                //client.Client.Blocking = false;
               // isConnected = true;
                Console.WriteLine("You are connected");
               // stream = client.GetStream();
                Task.Run(() => ReceiveMessageFromServer());
                return true;
            }
            catch
            {
                Console.WriteLine("Unable to connect to server. Please check your connection.");
               // isConnected = false;
                return false;
            }
        }

        public void SendMessageToServer(CommandEventArgs commandArgs)
        {
            //check connection here first
            if (!client.Connected)
            {
                DisconnectedFromServer?.Invoke(this, client.Connected);
                return;
               // ConnectToServer();
            }
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);
            {
                try
                {
                    //CommandEventArgs commandArgs = new CommandEventArgs() { CommandID = commandID, CommandArgs = args };
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(commandArgs);
                    //string message = ""; //make it in jason
                    mutex.WaitOne();
                    writer.Write(jsonString);
                    mutex.ReleaseMutex();
                    /*mutex.WaitOne();
                    jsonString = reader.ReadString();
                    mutex.ReleaseMutex();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                    
                    //CommandEventArgs commandArgs = new CommandEventArgs(); //make sure tanslate from jason
                    OnMessageFromServer?.Invoke(this, (CommandEventArgs)obj);*/
                   

                }
                catch { Debug.WriteLine("cant write to server"); }
            }
        }

        public void ReceiveMessageFromServer()
        {
            try
            {
                while (!stop)
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    try
                    {
                        if (stream.DataAvailable)
                        {
                            string jsonString = reader.ReadString();
                            //mutex.ReleaseMutex();
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandEventArgs>(jsonString);
                            //CommandEventArgs commandArgs = new CommandEventArgs(); //make sure tanslate from jason
                            if (obj != null)
                            {
                                OnMessageFromServer?.Invoke(this, (CommandEventArgs)obj);
                            }
                        }
                        else { Thread.Sleep(2); }
                        //mutex.WaitOne();
                    }
                    catch
                    {
                        break;
                        //Thread.Sleep(2);
                        //DisconnectedFromServer?.Invoke(this, client.Connected);
                    }
                }
                
            }
            catch { Debug.WriteLine("stopped listening loop");} //stop=true; }
        }

        public void DisconnectFromServer()
        {
            stop = true;
            //stream.Close();
           // SendMessageToServer(new CommandEventArgs() { CommandID = CommandEnum.DisconnectClientCommand, CommandArgs = new string[] { (client.GetHashCode()).ToString() } });
            DisconnectedFromServer?.Invoke(this, true);
        }
    }
}
