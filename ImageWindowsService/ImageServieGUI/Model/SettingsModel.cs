using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServieGUI.Communication;
using ImageService.Infrastructure.Enums;
using System.Diagnostics;
using ImageServieGUI.ViewModel;
using ImageService.Infrastructure.Event;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ImageServieGUI.Model
{
    class SettingsModel : IModel
    {
        
        public override event EventHandler<SettingsEventArgs> changeInModel;

        public SettingsModel()
        {
            GUIClient client = GUIClient.GetInstance();
            client.MessageReceived += MessageFromServer;
            Task.Run(()=>client.SendMessageToServer(CommandEnum.GetConfigCommand, new string[] { "" }));
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
            e.Handlers = splitSettings[4].Split(';'); ;
            Debug.WriteLine(e.OutputDir + e.SourceName + e.LogName + e.ThumbnailSize);
            return e;
        } 
        
        public override void MessageFromServer(object sender, CommandEventArgs args)
        {
            // int commadID = int.Parse(message[0].ToString());
            CommandEnum commandID = args.CommandID;
            if (commandID == CommandEnum.GetConfigCommand)
            {
                GUIClient client = (GUIClient)sender;
                string message = args.CommandArgs[0];
                Debug.WriteLine("In message from server settings model meessage recieved is" + message);
                string settings = message.Substring(1);
                SettingsEventArgs e = ParseSettingsFromString(settings);
                changeInModel?.Invoke(this, e);
            }    
        }

        public override void SendMessageToServer(CommandEnum commandID, string args)
        {
            GUIClient client = GUIClient.GetInstance();
            client.SendMessageToServer(commandID, new string[] { args });
        }
    }
}
