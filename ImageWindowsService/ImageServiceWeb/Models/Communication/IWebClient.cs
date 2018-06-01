using Communications.Channels;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWeb.Models.Communication
{
    public interface IWebClient
    {
        event EventHandler<CommandEventArgs> OnDataReceived;

        void SendCommand(CommandEnum commandID);
        bool Connect();
    }
}
