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
        public override event EventHandler<string> OnHandlerRemoved;
        public override event EventHandler<SettingsEventArgs> changeInModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsModel"/> class.
        /// </summary>
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

        /// <summary>
        /// Messages from server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        public override void MessageFromServer(object sender, CommandEventArgs args)
        {
            CommandEnum commandID = args.CommandID;
            if (commandID == CommandEnum.GetConfigCommand)
            {
                string jsonSettings = args.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsEventArgs>(jsonSettings);
                changeInModel?.Invoke(this, (SettingsEventArgs)obj);
            }    
            if (commandID == CommandEnum.RemoveHandlerCommand)
            {
                string jsonHandler = args.CommandArgs[0];
                OnHandlerRemoved?.Invoke(this, jsonHandler);
            }
        }

        /// <summary>
        /// Sends the message to server.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        public override void SendMessageToServer(CommandEnum commandID, string args)
        {
            GUIClient client = GUIClient.GetInstance();
            client.SendMessageToServer(commandID, new string[] { args });
        }
    }
}
