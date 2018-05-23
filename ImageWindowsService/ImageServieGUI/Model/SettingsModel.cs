using System;
using System.Threading.Tasks;
using ImageServieGUI.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using Infrastructure.Event;

namespace ImageServieGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        
        public override event EventHandler<SettingsEventArgs> changeInModel;

        public SettingsModel()
        {
            GUIClient client = GUIClient.GetInstance();
            client.MessageReceived += MessageFromServer;
            Task.Run(()=>client.SendMessageToServer(CommandEnum.GetConfigCommand, new string[] { "" }));
        }

        /**private SettingsEventArgs ParseSettingsFromString(string settings)
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
        } */
        
        public override void MessageFromServer(object sender, CommandEventArgs args)
        {
            CommandEnum commandID = args.CommandID;
            if (commandID == CommandEnum.GetConfigCommand)
            {
                string jsonSettings = args.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsEventArgs>(jsonSettings);
                changeInModel?.Invoke(this, (SettingsEventArgs)obj);
            }    
        }

        public override void SendMessageToServer(CommandEnum commandID, string args)
        {
            GUIClient client = GUIClient.GetInstance();
            client.SendMessageToServer(commandID, new string[] { args });
        }
    }
}
