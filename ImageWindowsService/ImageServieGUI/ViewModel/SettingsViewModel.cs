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
using ImageService.Infrastructure.Enums;
using ImageServieGUI.Model;
using System.Windows.Data;

namespace ImageServieGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private IModel settingsModel;
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

        public ICommand AddHandler { get; set; }
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
            handlers = new ObservableCollection<HandlerDir>();
            this.RemoveHandler = new DelegateCommand<object>(this.OnRemoveHandler, this.CanRemoveHandler);
            this.AddHandler = new DelegateCommand(this.OnAddHandler);
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
            Task.Run(() => { settingsModel.SendMessageToServer(CommandEnum.RemoveHandlerCommand, selectedHandler.Name); });
            handlers.Remove(selectedHandler);
            //settingsModel.MessageFromServer()
            selectedHandler = null;
        }
        private void OnAddHandler() {
            handlers.Add(new HandlerDir() { Name = "Liora" });
        }

        public void SettingsEvent(object sender, SettingsEventArgs e)
        {
            Debug.WriteLine("Settings Event in settings view model");
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
