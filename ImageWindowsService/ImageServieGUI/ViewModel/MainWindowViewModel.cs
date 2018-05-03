using ImageServieGUI.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageServieGUI.ViewModel
{
    class MainWindowViewModel
    {
        //private TcpClient client;
        public bool ServerConnected { get; set; }
        public Brush WindowColor { get; set; }

        public MainWindowViewModel()
        {
            ServerConnected = CLient.GetInstance().isConnected;
            WindowColor = Brushes.Gray;
            if (ServerConnected) WindowColor = Brushes.Pink;
        }

        /*public void ConnectToServer()
        {
            WindowColor = Brushes.Gray;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            try
            {
                client.Connect(ep);
                WindowColor = Brushes.Pink;
                Console.WriteLine("You are connected");
            }
            catch
            {
                Console.WriteLine("Unable to connect to server. Please check your connection.");
            }
            
            /*using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                Console.Write("Please enter a number: ");
                
                //int num = int.Parse(Console.ReadLine());
                //writer.Write(num);
                // Get result from server
                //nt result = reader.ReadInt32();
                //Console.WriteLine("Result = {0}", result);
            }
            
        }*/
       /* public void DisconnectFromServer()
        {
            WindowColor = Brushes.Gray;
            client.Close();
        }*/
    }
}
