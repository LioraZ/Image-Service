using ImageService.Infrastructure.Enums;
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
    class CLient
    {
        private static CLient instance = null;
        private TcpClient client;
        public bool isConnected;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<bool> CheckConnection;

        public static CLient GetInstance()
        {
            if (instance == null) instance = new CLient();
            return instance;
        }

        private CLient()
        {
            ConnectToServer();
        }

        public bool ConnectToServer()
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
        }

        public void SendMessageToServer(CommandEnum e, string args)
        {
            if (!client.Connected)
            {
                ConnectToServer();
                CheckConnection?.Invoke(this, client.Connected);
                //return;
             //   if (!ConnectToServer()) return;
            }
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                writer.Write((int)e);
                writer.Write(args);
                Console.WriteLine("send message to server waiting for response");
                //string message = reader.ReadString();
                string message = reader.ReadString();
                if (message == "") return;
                Console.WriteLine(message + " recieved");
                Task.Run(()=>{ MessageReceived?.Invoke(this, message); });
                //Console.WriteLine(message + "recieved");
                //MessageReceived?.Invoke(this, message);
                //writer.Write("Settings received" + settings);
                // SettingModel.
            }
        }
        public string GetMessage()
        {
            return "";
        }

        public void DisconnectFromServer()
        {
            client.Close();
        }
    }
}
