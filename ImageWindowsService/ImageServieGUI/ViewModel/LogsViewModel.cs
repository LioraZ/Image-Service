using ImageService.Logging.Model;
using ImageServieGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

/// <summary>
/// The ViewModel namespace.
/// </summary>
namespace ImageServieGUI.ViewModel
{
    /// <summary>
    /// Class LogsViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    class LogsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The logs model
        /// </summary>
        private LogsModel logsModel;
        public ObservableCollection<MessageRecievedEventArgs> logs;
        /// <summary>
        /// Gets or sets the logs.
        /// </summary>
        /// <value>The logs.</value>
        public ObservableCollection<MessageRecievedEventArgs> Logs
        {
            get { return logs; }
            set
            {
                if (value != this.logs)
                    logs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Logs"));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsViewModel"/> class.
        /// </summary>
        public LogsViewModel()
        {
            logsModel = new LogsModel();
            logsModel.ReceivedLog += OnLogReceived;
            logs = new ObservableCollection<MessageRecievedEventArgs>();
            //logs.Add(new MessageRecievedEventArgs("Liora", MessageTypeEnum.INFO));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles the <see cref="E:LogReceived" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="log">The <see cref="MessageRecievedEventArgs"/> instance containing the event data.</param>
        public void OnLogReceived(object sender, MessageRecievedEventArgs log)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                if (logs.Count >= 0) logs.Insert(0, log);
                else logs.Add(log);
            });
        }

        //public void OnAllLogsReceived(object sender, List<>)
    }
}
