using ImageService.Commands;
using ImageService.Infrastructure.Enums;
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
            StringBuilder sb = new StringBuilder();
            sb.Append(((int)CommandEnum.GetConfigCommand).ToString());
            sb.Append(ConfigurationManager.AppSettings["OutputDir"] + "|");
            sb.Append(ConfigurationManager.AppSettings["SourceName"] + "|");
            sb.Append(ConfigurationManager.AppSettings["LogName"] + "|");
            sb.Append(ConfigurationManager.AppSettings["ThumbnailSize"] + "|");
            sb.Append(ConfigurationManager.AppSettings["Handler"]);
            result = true;
            return sb.ToString();
        }
    }
}
