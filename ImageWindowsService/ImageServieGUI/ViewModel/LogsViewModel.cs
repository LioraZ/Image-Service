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
        public ObservableCollection<LogInfo> logs;
        public ObservableCollection<LogInfo> Logs
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
            logs = new ObservableCollection<LogInfo>();
            logs.Add(new LogInfo() { Name = "Liora", LogType = 1 });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
