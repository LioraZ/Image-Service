using ImageServieGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool serverConnected;
        public bool ServerConnected {
            get { return serverConnected; }
            set
            {
                serverConnected = value;
                if (serverConnected) WindowColor = Brushes.Pink;
                else WindowColor = Brushes.Gray;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WindowColor"));
            }
        }
        public Brush WindowColor { get; set; }

        public MainWindowViewModel()
        {
            GUIClient client = GUIClient.GetInstance();
            ServerConnected = client.isConnected;
            client.CheckConnection += CheckConnection;
            WindowColor = Brushes.Gray;
            if (ServerConnected) WindowColor = Brushes.MediumVioletRed;
        }

        public void CheckConnection(object sender, bool isConnected)
        {
            ServerConnected = isConnected;
        }
        public event PropertyChangedEventHandler PropertyChanged;

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
