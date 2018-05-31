using Communications.Channels;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWeb.Models.Communication
{
    public interface IWebClient
    {
        void SendCommand(CommandEnum commandID);
        bool Connect();
    }
}
