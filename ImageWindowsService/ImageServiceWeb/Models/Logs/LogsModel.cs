using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageService.Logging.Model;
using ImageServiceWeb.Models.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImageServiceWeb.Models.Logs
{
    public class LogsModel
    {
        private IWebClient client;

        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public List<MessageRecievedEventArgs> Logs;

        public LogsModel(IWebClient webClient)
        {
            client = webClient;
            client.OnDataReceived += GetData;
            //Task.Run(() => client.SendCommand(CommandEnum.GetAllLogsCommand));
            client.SendCommand(CommandEnum.GetAllLogsCommand);
        }

        private void GetData(object sender, CommandEventArgs e)
        {
            CommandEnum commandID = e.CommandID;
            if (commandID == CommandEnum.GetAllLogsCommand)
            {
                string message = e.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(message);
                Logs = obj;
                //List<MessageRecievedEventArgs> logs;
                //= (List<MessageRecievedEventArgs>)obj;
                //logs.Reverse();
                //Logs = logs;
                /*foreach (MessageRecievedEventArgs log in (List<MessageRecievedEventArgs>)obj)
                {
                    //var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageRecievedEventArgs>(e.CommandArgs[0]);
                    MessageRecievedEventArgs logInfo = (MessageRecievedEventArgs)Newtonsoft.Json.JsonConvert.DeserializeObject<MessageRecievedEventArgs>("");
                    if (Logs.Count >= 0) Logs.Insert(0, logInfo);
                    else Logs.Add(logInfo);
                }*/
            }
            /*else if (commandID == CommandEnum.LogCommand)
            {
                string message = e.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageRecievedEventArgs>(e.CommandArgs[0]);
                MessageRecievedEventArgs logInfo = (MessageRecievedEventArgs)obj;
                if (Logs.Count >= 0) Logs.Insert(0, logInfo);
                else Logs.Add(logInfo);
            }*/
        }
    }
}