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
        /*private string outputDir;
        private string sourceName;
        private string logName;
        private int thumbnailSize;*/
        private List<string> handlers;
        public override event EventHandler<SettingsEventArgs> changeInModel;

        /*public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }*/

        public SettingsModel()
        {
            CLient client = CLient.GetInstance();
            client.MessageReceived += MessageFromServer;//change to not static event
            client.SendMessageToServer(CommandEnum.GetConfigCommand);
            handlers = new List<string>();
            changeInModel += WhenInvokedTet;
            changeInModel += SettingsModel_changeInModel;
           
            //InvokeEvent(new SettingsEventArgs());
            //changeInModel += InvokeEvent;
        }

        private void SettingsModel_changeInModel(object sender, SettingsEventArgs e)
        {
            Debug.WriteLine("Self generated handler activated");
            //throw new NotImplementedException();
        }

        private SettingsEventArgs ParseSettingsFromString(string settings)
        {
            SettingsEventArgs e = new SettingsEventArgs();
            Debug.WriteLine("Starting to parse settings");
            string[] splitSettings = settings.Split('|');
            e.OutputDir = splitSettings[0];
            e.SourceName = splitSettings[1];
            e.LogName = splitSettings[2];
            e.ThumbnailSize = int.Parse(splitSettings[3]);
            //string[] splitHandlers = splitSettings[4].Split(';');
            //foreach (string handler in splitHandlers) { handlers.Add(handler); Debug.WriteLine(handler); }
            Debug.WriteLine(e.OutputDir + e.SourceName + e.LogName + e.ThumbnailSize);
            //InvokeEvent(e);
            //Debug.WriteLine("after event nvoked");
            return e;
        }
        public void WhenInvokedTet(object sender, SettingsEventArgs e)
        {
            Debug.WriteLine("Testing event successful");
        }
        public override void MessageFromServer(object sender, string message)
        {
            CLient client = (CLient)sender;
            string settings = message.Substring(1);
            SettingsEventArgs e = ParseSettingsFromString(settings);
            changeInModel?.Invoke(this, e);
            //InvokeEvent(e);
            /*if (message[0] == (int)CommandEnum.GetConfigCommand)
            {
                CLient client = (CLient)sender;
                string settings = message.Substring(1);
                ParseSettingsFromString(settings);
            }*/
        }
        protected override void InvokeEvent(SettingsEventArgs e)
        {
            base.InvokeEvent(e);
        }
        /*public void InvokeEvent(object sender, SettingsEventArgs e)
        {
            Debug.WriteLine("I was invoked in settings model");
        }*/
    }
}
