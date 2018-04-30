using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageServieGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<HandlerDir> handlers = new ObservableCollection<HandlerDir>();
        public ObservableCollection<HandlerDir> Handlers
        {
            get { return handlers; }
            set
            {
                if (value != this.handlers)
                    handlers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Handlers"));
            }
        }

        private HandlerDir selectedHandler;
        public HandlerDir SelectedHandler
        {
            get { return selectedHandler; }
            set
            {
                if (value != this.selectedHandler)
                    selectedHandler = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedHandler"));
            }
        }

        private int thumbnailSize = 120;
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize {
            get { return thumbnailSize; }
            set { thumbnailSize = value; }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            handlers.Add(new HandlerDir() { Name = "Liora" });
        }
        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            /*if (lbUsers.SelectedItem != null)
                handlers.Remove(lbUsers.SelectedItem as string);*/
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
