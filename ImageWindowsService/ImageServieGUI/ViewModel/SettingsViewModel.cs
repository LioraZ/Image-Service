using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using ImageServieGUI.Model;

namespace ImageServieGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsModel settingsModel;
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
                {
                    selectedHandler = value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedHandler"));
                RaiseCanExecuteChange();
            }
        }

        public ICommand AddHandler { get; set; }
        public ICommand RemoveHandler { get; set; }

        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }

        public SettingsViewModel()
        {
            this.settingsModel = new SettingsModel();
            this.RemoveHandler = new DelegateCommand<object>(this.OnRemoveHandler, this.CanRemoveHandler);
            this.AddHandler = new DelegateCommand(this.OnAddHandler);
            handlers.Add(new HandlerDir() { Name = "Liora" });
            handlers.Add(new HandlerDir() { Name = "Atara" });
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                Debug.WriteLine("GetSelected");
                return isSelected;
            }
            set
            {
                Debug.WriteLine("SetSelected");
                if (value != isSelected) isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private bool CanRemoveHandler(object obj)
        {
            if (selectedHandler != null)
            {
                Debug.WriteLine(selectedHandler.ToString());
                return true;
            }
            Debug.WriteLine("nothing selected yet!");
            return false;
        }
        private void RaiseCanExecuteChange()
        {
            DelegateCommand<object> command = RemoveHandler as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }
        private void OnRemoveHandler(object obj)
        {
            handlers.Remove(selectedHandler);
            selectedHandler = null;
        }
        private void OnAddHandler() {
            handlers.Add(new HandlerDir() { Name = "Liora" });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
