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
                if (serverConnected) WindowColor = Brushes.MediumVioletRed;
                else WindowColor = Brushes.Gray;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WindowColor"));
            }
        }
        public Brush WindowColor { get; set; }

        public MainWindowViewModel()
        {
            GUIClient client = GUIClient.GetInstance();
            ServerConnected = GUIClient.isConnected;
            client.CheckConnection += CheckConnection;
            WindowColor = Brushes.Gray;
            if (ServerConnected) WindowColor = Brushes.MediumVioletRed;
        }

        public void CheckConnection(object sender, bool isConnected)
        {
            ServerConnected = isConnected;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
