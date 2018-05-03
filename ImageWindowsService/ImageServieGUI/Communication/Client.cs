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
        public static event EventHandler<string> MessageReceived; 

        public CLient GetInstance()
        {
            if (instance == null) instance = new CLient();
            return instance;
        }

        private CLient()
        {
            ConnectToServer();
        }

        public void ConnectToServer()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            try
            {
                client.Connect(ep);
                Console.WriteLine("You are connected");
                MessageReceived?.Invoke(this, "initial connection");
            }
            catch
            {
                Console.WriteLine("Unable to connect to server. Please check your connection.");
            }
        }
        public void EstablishStream()
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                writer.Write("settings");
                Console.Write("waiting to receives settings...");
                string settings = reader.ReadString();
                Console.WriteLine(settings + "recieved");
                writer.Write("Settings received" + settings);
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
