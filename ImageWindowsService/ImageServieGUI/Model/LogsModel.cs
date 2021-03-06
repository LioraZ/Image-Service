﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Model;
using ImageServieGUI.Communication;
using ImageService.Infrastructure.Event;

namespace ImageServieGUI.Model
{
    /// <summary>
    /// Class LogsModel.
    /// </summary>
    class LogsModel
    {
        public event EventHandler<MessageRecievedEventArgs> ReceivedLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsModel"/> class.
        /// </summary>
        public LogsModel()
        {
            GUIClient client = GUIClient.GetInstance();
            client.MessageReceived += MessageFromServer;
            Task.Run(()=>client.SendMessageToServer(CommandEnum.GetAllLogsCommand, new string[] { }));
        }

        /// <summary>
        /// Parses the log from string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>MessageRecievedEventArgs.</returns>
        public MessageRecievedEventArgs ParseLogFromString(string log)
        {
            string[] args = log.Split(';');
            string message = args[0];
            MessageTypeEnum status = (MessageTypeEnum)(int.Parse(args[1]));
            return new MessageRecievedEventArgs(message, status);
        }

        /// <summary>
        /// Messages from server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CommandEventArgs" /> instance containing the event data.</param>
        public void MessageFromServer(object sender, CommandEventArgs args)
        {
            CommandEnum commandID = args.CommandID;
            if (commandID == CommandEnum.GetAllLogsCommand)
            {
                GUIClient client = (GUIClient)sender;
                string message = args.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(message);
                List<MessageRecievedEventArgs> logs = (List<MessageRecievedEventArgs>)obj;
                logs.Reverse();
                foreach (MessageRecievedEventArgs log in (List<MessageRecievedEventArgs>)obj) { ReceivedLog?.Invoke(this, log); }
                //for (int i = logs.Count - 1; i >= 0; i++) {  ReceivedLog?.Invoke(this, logs[i]); }
            }
            else if (commandID == CommandEnum.LogCommand)
            {
                GUIClient client = (GUIClient)sender;
                string message = args.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageRecievedEventArgs>(args.CommandArgs[0]);
                //string log = message.Substring(1);
                // string log = message;
                // MessageRecievedEventArgs logInfo = ParseLogFromString(log);
                MessageRecievedEventArgs logInfo = (MessageRecievedEventArgs)obj;
                ReceivedLog?.Invoke(this, logInfo);
            }
        }
    }
}
