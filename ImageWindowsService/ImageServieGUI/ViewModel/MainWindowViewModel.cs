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
    /// <summary>
    /// Class MainWindowViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The server connected
        /// </summary>
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
        /// <summary>
        /// Gets or sets the color of the window.
        /// </summary>
        /// <value>The color of the window.</value>
        public Brush WindowColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            GUIClient client = GUIClient.GetInstance();
            ServerConnected = GUIClient.isConnected;
            client.CheckConnection += CheckConnection;
            WindowColor = Brushes.Gray;
            if (ServerConnected) WindowColor = Brushes.MediumVioletRed;
        }

        /// <summary>
        /// Checks the connection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="isConnected">if set to <c>true</c> [is connected].</param>
        public void CheckConnection(object sender, bool isConnected)
        {
            ServerConnected = isConnected;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
