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
            }
        }
    }
}