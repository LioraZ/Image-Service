using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    class LogCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            string message = args[0];
            result = true;
            try
            {
                int messageType = int.Parse(args[1]);
                return message + ";" + messageType;
            }
            catch { }
            return message + ";" + ((int)MessageTypeEnum.INFO).ToString();
        }
    }
}
