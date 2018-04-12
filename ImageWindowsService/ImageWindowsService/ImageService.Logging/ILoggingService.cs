using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageReceived;
        void Log(string message, MessageTypeEnum type);
        EventLogEntryType GetMessageType(MessageTypeEnum type);
    }
}
