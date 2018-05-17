using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Model;
using ImageServieGUI.Communication;

namespace ImageServieGUI.Model
{
    class LogsModel
    {
        public event EventHandler<MessageRecievedEventArgs> ReceivedLog;

        public LogsModel()
        {
            CLient client = CLient.GetInstance();
            client.MessageReceived += MessageFromServer;
        }

        public MessageRecievedEventArgs ParseLogFromString(string log)
        {
            string[] args = log.Split(';');
            string message = args[0];
            MessageTypeEnum status = (MessageTypeEnum)(int.Parse(args[1]));
            return new MessageRecievedEventArgs(message, status);
        }

        public void MessageFromServer(object sender, string message)
        {
            int commandID = int.Parse(message[0].ToString());
            if (commandID == (int)CommandEnum.LogCommand)
            {
                CLient client = (CLient)sender;
                string log = message.Substring(1);
                MessageRecievedEventArgs args = ParseLogFromString(log);
                ReceivedLog?.Invoke(this, args);
            }

        }
    }
}
