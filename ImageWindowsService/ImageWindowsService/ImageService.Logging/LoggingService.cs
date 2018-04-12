
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageReceived;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageReceived?.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
        public EventLogEntryType GetMessageType(MessageTypeEnum type)
        {
            switch(type)
            {
                case (MessageTypeEnum.INFO): return EventLogEntryType.Information;
                case (MessageTypeEnum.WARNING): return EventLogEntryType.Warning;
                case (MessageTypeEnum.FAIL): return EventLogEntryType.FailureAudit;
            }
            return EventLogEntryType.Information;
        }
    }
}
