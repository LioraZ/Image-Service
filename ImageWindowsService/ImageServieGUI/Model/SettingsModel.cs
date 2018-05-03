using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServieGUI.Communication;
using ImageService.Infrastructure.Enums;
using System.Diagnostics;

namespace ImageServieGUI.Model
{
    class SettingsModel : IModel
    {
        private string outputDir;
        private string sourceName;
        private string logName;
        private int thumbnailSize;
        private List<string> handlers;
        public event EventHandler changeInModel;

        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }

        public SettingsModel()
        {
            CLient client = CLient.GetInstance();
            client.MessageReceived += MessageFromServer;//change to not static event
            client.SendMessageToServer(CommandEnum.GetConfigCommand);
            handlers = new List<string>();
        }

        private void ParseSettingsFromString(string settings)
        {

            Debug.WriteLine("Starting to paarse settings");
            string[] splitSettings = settings.Split('|');
            OutputDir = splitSettings[0];
            SourceName = splitSettings[1];
            LogName = splitSettings[2];
            ThumbnailSize = int.Parse(splitSettings[3]);
            //string[] splitHandlers = splitSettings[4].Split(';');
            //foreach (string handler in splitHandlers) { handlers.Add(handler); Debug.WriteLine(handler); }
            Debug.Write(OutputDir + SourceName + LogName + ThumbnailSize);


        }
        public void MessageFromServer(object sender, string message)
        {
            CLient client = (CLient)sender;
            string settings = message.Substring(1);
            ParseSettingsFromString(settings);
            /*if (message[0] == (int)CommandEnum.GetConfigCommand)
            {
                CLient client = (CLient)sender;
                string settings = message.Substring(1);
                ParseSettingsFromString(settings);
            }*/
        }
    }
}
