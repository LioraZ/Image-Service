using ImageService.Commands;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    class RemoveHandlerCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            int commandID = (int)CommandEnum.RemoveHandlerCommand; 
            string h = args[0];
            string handlers = ConfigurationManager.AppSettings["Handler"];
            int handlerIndex = handlers.IndexOf(h);
            if (handlerIndex >= 0)
            {
                string newHandlersList = handlers.Remove(handlerIndex);//make sure to change
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Handler"].Value = newHandlersList;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                //DirectoryHandler ha = new DirectoryHandler();
                //h.DirectoryClose(h);
                

                result = true;
                return commandID + h + "successfully removed from handlers list";
            }
            else
            {
                result = false;
                return commandID + "couldn't remove handler";
            }
           
            //throw new NotImplementedException();
        }
    }
}
