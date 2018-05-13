using ImageService.Commands;
using ImageService.Infrastructure.Enums;
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
            int messageType = int.Parse(args[1]);
            result = true;
            return CommandEnum.LogCommand.ToString() + message + ";" + messageType;  
        }


    }
}
