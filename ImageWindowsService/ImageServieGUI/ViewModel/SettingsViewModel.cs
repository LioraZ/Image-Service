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
        /// <summary>
        /// The settings model
        /// </summary>
        private ISettingsModel settingsModel;
        /// <summary>
        /// The handlers
        /// </summary>
        public ObservableCollection<HandlerDir> handlers;
        /// <summary>
        /// Gets or sets the handlers.
        /// </summary>
        /// <value>The handlers.</value>
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

        /// <summary>
        /// The selected handler
        /// </summary>
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

        /// <summary>
        /// Gets or sets the remove handler.
        /// </summary>
        /// <value>The remove handler.</value>
        public ICommand RemoveHandler { get; set; }

        private string outputDir = String.Empty;
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
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

        /// <summary>
        /// The source name
        /// </summary>
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

        /// <summary>
        /// The log name
        /// </summary>
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

        /// <summary>
        /// The thumbnail size
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            this.settingsModel = new SettingsModel();
            settingsModel.changeInModel += this.SettingsEvent;
            settingsModel.OnHandlerRemoved += RemoveSelectedHandler;
            handlers = new ObservableCollection<HandlerDir>();
            this.RemoveHandler = new DelegateCommand<object>(this.OnRemoveHandler, this.CanRemoveHandler);
        }

        /// <summary>
        /// The is selected
        /// </summary>
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

        /// <summary>
        /// Determines whether this instance [can remove handler] the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if this instance [can remove handler] the specified object; otherwise, <c>false</c>.</returns>
        private bool CanRemoveHandler(object obj)
        {
            return selectedHandler != null;
        }
        /// <summary>
        /// Raises the can execute change.
        /// </summary>
        private void RaiseCanExecuteChange()
        {
            DelegateCommand<object> command = RemoveHandler as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }
        /// <summary>
        /// Called when [remove handler].
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnRemoveHandler(object obj)
        {
            string handler = selectedHandler.Name;
            selectedHandler = null;
            Task.Run(() => { settingsModel.SendMessageToServer(CommandEnum.RemoveHandlerCommand, handler); });
            //handlers.Remove(selectedHandler);
            
        }
        /// <summary>
        /// Removes the selected handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        private void RemoveSelectedHandler(object sender, string handler)
        {
            HandlerDir removedHandler = new HandlerDir() { Name = handler };
            foreach (HandlerDir h in handlers) {  if (removedHandler.Name == h.Name) removedHandler = h; }
            try  { App.Current.Dispatcher.Invoke((System.Action)delegate { handlers.Remove(removedHandler);}); }
            catch { }
        }

        /// <summary>
        /// Settingses the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SettingsEventArgs"/> instance containing the event data.</param>
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
