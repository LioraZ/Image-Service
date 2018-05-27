using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using ImageService.Infrastructure.Enums;
using ImageServieGUI.Model;
using Infrastructure.Event;

namespace ImageServieGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel settingsModel;
        public ObservableCollection<HandlerDir> handlers;
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
        
        public ICommand RemoveHandler { get; set; }

        private string outputDir = String.Empty;
        public string OutputDir
        {
            get { return outputDir; }
            set
            {
                if (value != outputDir)
                    outputDir = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OutputDir"));
            }
        }

        private string sourceName = String.Empty;
        public string SourceName
        {
            get { return sourceName; }
            set
            {
                if (value != sourceName)
                    sourceName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SourceName"));
            }
        }

        private string logName = String.Empty;
        public string LogName
        {
            get { return logName; }
            set
            {
                if (value != logName)
                    logName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LogName"));
            }
        }

        private int thumbnailSize = 0;
        public int ThumbnailSize
        {
            get { return thumbnailSize; }
            set
            {
                if (value != thumbnailSize)
                    thumbnailSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ThumbnailSize"));
            }
        }

        public SettingsViewModel()
        {
            this.settingsModel = new SettingsModel();
            settingsModel.changeInModel += this.SettingsEvent;
            settingsModel.OnHandlerRemoved += RemoveSelectedHandler;
            handlers = new ObservableCollection<HandlerDir>();
            this.RemoveHandler = new DelegateCommand<object>(this.OnRemoveHandler, this.CanRemoveHandler);
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected) isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private bool CanRemoveHandler(object obj)
        {
            return selectedHandler != null;
        }
        private void RaiseCanExecuteChange()
        {
            DelegateCommand<object> command = RemoveHandler as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }
        private void OnRemoveHandler(object obj)
        {
            string handler = selectedHandler.Name;
            Task.Run(() => { settingsModel.SendMessageToServer(CommandEnum.RemoveHandlerCommand, handler); });
            //handlers.Remove(selectedHandler);
            selectedHandler = null;
        }
        private void RemoveSelectedHandler(object sender, string handler)
        {
            handlers.Remove(new HandlerDir() { Name = handler });
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Handlers"));
            //selectedHandler = null;
        }
        private void OnAddHandler() {
            handlers.Add(new HandlerDir() { Name = "Liora" });
        }

        public void SettingsEvent(object sender, SettingsEventArgs e)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                OutputDir = e.OutputDir;
                SourceName = e.SourceName;
                LogName = e.LogName;
                ThumbnailSize = e.ThumbnailSize;
                foreach (string handler in e.Handlers)
                {
                    handlers.Add(new HandlerDir() { Name = handler });
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
