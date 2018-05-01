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

        public ICommand AddHandler { get; private set; }
        public ICommand RemoveHandler { get; private set; }

        private int thumbnailSize = 120;
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize {
            get { return thumbnailSize; }
            set { thumbnailSize = value; }
        }

        public SettingsViewModel()
        {
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
        /*private void addHandlerButton(object sender, RoutedEventArgs e)
        {
            handlers.Add(new HandlerDir() { Name = "Liora" });
        }
        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
         
            if (SelectedItem != null)
                handlers.Remove(lbUsers.SelectedItem as string);
        }*/
        private bool CanRemoveHandler(object obj)
        {
            return IsSelected;
            if (obj == null)  return false;
           /* foreach (HandlerDir h in Handlers)
                if (Handlers.SelectedItems.Contains(h))
                    h.Value = true;
                else
                    MyObject.Value = false;*/
            /*if (this.QuestionnaireViewModel.Questionnaire.Age < 0 || this.QuestionnaireViewModel.Questionnaire.Age > 120)
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.QuestionnaireViewModel.Questionnaire.Quest))
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.QuestionnaireViewModel.Questionnaire.FavoriteColor))
            {
                return false;
            }*/
            return true;
        }
        private void OnRemoveHandler(object obj)
        {
            Debug.WriteLine("handler removed");
        }
        private void OnAddHandler() {
            handlers.Add(new HandlerDir() { Name = "Liora" });
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
