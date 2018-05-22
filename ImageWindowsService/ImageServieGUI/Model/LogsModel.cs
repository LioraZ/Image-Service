using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Model;
using ImageServieGUI.Communication;
using ImageService.Infrastructure.Event;
using System.Diagnostics;

namespace ImageServieGUI.Model
{
    class LogsModel
    {
        public event EventHandler<MessageRecievedEventArgs> ReceivedLog;

        public LogsModel()
        {
            GUIClient client = GUIClient.GetInstance();
            client.MessageReceived += MessageFromServer;
            Task.Run(()=>client.SendMessageToServer(CommandEnum.GetAllLogsCommand, new string[] { }));
        }

        public MessageRecievedEventArgs ParseLogFromString(string log)
        {
            string[] args = log.Split(';');
            string message = args[0];
            MessageTypeEnum status = (MessageTypeEnum)(int.Parse(args[1]));
            return new MessageRecievedEventArgs(message, status);
        }

        public void MessageFromServer(object sender, CommandEventArgs args)
        {
            CommandEnum commandID = args.CommandID;
            if (commandID == CommandEnum.GetAllLogsCommand)
            {
                GUIClient client = (GUIClient)sender;
                string message = args.CommandArgs[0];
                Debug.WriteLine("In message from server logs model message recieved is" + message);
                //string allLogs = message.Substring(1);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(message);
                foreach (MessageRecievedEventArgs log in (List<MessageRecievedEventArgs>)obj)
                {
                    ReceivedLog?.Invoke(this, log);
                }
            }
            else if (commandID == CommandEnum.LogCommand)
            {
                GUIClient client = (GUIClient)sender;
                string message = args.CommandArgs[0];
                Debug.WriteLine("In message from server logs model message recieved is" + message);
                string log = message.Substring(1);
                MessageRecievedEventArgs logInfo = ParseLogFromString(log);
                ReceivedLog?.Invoke(this, logInfo);
                //SettingsEventArgs e = ParseSettingsFromString(settings);
                //changeInModel?.Invoke(this, e);
            }
        }
    }
}
