using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServieGUI.Communication;

namespace ImageServieGUI.Model
{
    class SettingsModel
    {
        private static SettingsModel instance = null;

        private string outputDir;
        private string sourceName;
        private string logName;
        private int thumbnailSize;
        private List<string> handler;

        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }

        public SettingsModel()
        {

            CLient.MessageReceived += MessageFromClient;//change to not static event
            //CLient.sendMessage("settings");
        }

        /*public SettingsModel GetInstance()
        {
            if (instance == null)
            {
                instance = new SettingsModel();
            }
        }*/

        public static void ReceiveSettings(string settings)
        {
            
        }

        private void ParseSettingsFromString(string settings)
        {

        }
        public void MessageFromClient(object sender, string message)
        {
            if (message == "settings")
            {
                CLient client = (CLient)sender;
                string settings = client.GetMessage();
                ParseSettingsFromString(settings);
            }
        }
    }
}
