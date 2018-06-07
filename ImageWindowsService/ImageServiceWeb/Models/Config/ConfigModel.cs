using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageServiceWeb.Models.Communication;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImageServiceWeb.Models.Config
{
    public class ConfigModel
    {
        private IWebClient client;

        [DataType(DataType.Text)]
        [Display(Name = "Output Directory")]
        public string OutputDir { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Size")]
        public int ThumbnailSize { get; set; }

        //private string[] handlers;
        [DataType(DataType.Text)]
        [Display(Name = "Directory Handlers")]
        public string[] Handlers { get; set; }


        public ConfigModel(IWebClient webClient)
        {
            client = webClient;
            client.OnDataReceived += GetData;
            //Task.Run(()=> client.SendCommand(CommandEnum.GetConfigCommand));
            client.SendCommand(CommandEnum.GetConfigCommand);
        }

        private void GetData(object sender, CommandEventArgs e)
        {
            if (e.CommandID == CommandEnum.GetConfigCommand)
            {
                string jsonSettings = e.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsEventArgs>(jsonSettings);
                SettingsEventArgs settings = (SettingsEventArgs)obj;
                OutputDir = settings.OutputDir;
                SourceName = settings.SourceName;
                LogName = settings.LogName;
                ThumbnailSize = settings.ThumbnailSize;
                Handlers = settings.Handlers;
            }
        }
    }
}