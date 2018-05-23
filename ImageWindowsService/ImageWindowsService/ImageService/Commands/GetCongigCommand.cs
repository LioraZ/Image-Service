using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    class GetCongigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            SettingsEventArgs settings = new SettingsEventArgs(); 
            settings.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            settings.SourceName = ConfigurationManager.AppSettings["SourceName"];
            settings.LogName =  ConfigurationManager.AppSettings["LogName"];
            settings.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]); //might throw exception
            string handlers = ConfigurationManager.AppSettings["Handler"];
            settings.Handlers = handlers.Split(';');
            result = true;
            return Newtonsoft.Json.JsonConvert.SerializeObject(settings); ;
        }
    }
}
