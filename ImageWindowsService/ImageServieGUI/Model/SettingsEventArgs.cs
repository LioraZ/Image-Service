using ImageServieGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServieGUI.Model
{
    class SettingsEventArgs : EventArgs
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }
        public string[] Handlers { get; set; }
    }
}
