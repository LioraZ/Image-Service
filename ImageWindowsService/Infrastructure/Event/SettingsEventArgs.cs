using System;

namespace Infrastructure.Event
{
    public class SettingsEventArgs : EventArgs
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }
        public string[] Handlers { get; set; }
    }
}
