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

namespace ImageServieGUI.ViewModel
{
    class LogsViewModel : INotifyPropertyChanged
    {
        private LogsModel logsModel;
        public ObservableCollection<MessageRecievedEventArgs> logs;
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


        /*public Brush LogTypeColor
        {
            get
            {
                switch (LogInfo.LogType)
                {
                    case 1:
                        return Brushes.LightGreen;
                    case 2:
                        return Brushes.Red;
                    case 3:
                        return Brushes.Yellow;
                }
                return Brushes.Transparent;
            }
        }*/

        public LogsViewModel()
        {
            logsModel = new LogsModel();
            logsModel.ReceivedLog += OnLogReceived;
            logs = new ObservableCollection<MessageRecievedEventArgs>();
            //logs.Add(new MessageRecievedEventArgs("Liora", MessageTypeEnum.INFO));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnLogReceived(object sender, MessageRecievedEventArgs log)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                logs.Add(log);
            });
        }

        //public void OnAllLogsReceived(object sender, List<>)
    }
}
