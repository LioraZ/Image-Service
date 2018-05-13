using ImageService.Logging.Model;
using System;
using System.Diagnostics;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageReceived;    // The Event To Write The Logger's Message.

        /// <summary>
        /// The method invokes the MessageReceived event to log a message.
        /// </summary>
        /// <param name="message">The given message (to be logged)</param>
        /// <param name="type">The message type</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageReceived?.Invoke(this, new MessageRecievedEventArgs(message, type));
        }

        /// <summary>
        /// The method converts the MessageType enum too the system's EventLogEntryType.
        /// </summary>
        /// <param name="type">The given MessageTypeEnum</param>
        /// <returns>The converted EventLogEntryType</returns>
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
