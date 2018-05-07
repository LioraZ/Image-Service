using ImageService.Commands;
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
            string h = args[0];
            string handlers = ConfigurationManager.AppSettings["Handler"];
            int handlerIndex = handlers.IndexOf(h);
            if (handlerIndex > 0)
            {
                string newHandlersList = handlers.Remove(handlerIndex);
                ConfigurationManager.AppSettings["Handler"] = newHandlersList;
                result = true;
                return h + "successfully removed fro handlers list";
            }
            else
            {
                result = false;
                return "couldn't remove handler";
            }
           
            //throw new NotImplementedException();
        }
    }
}
