using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ImageService.Logging.Model
{
    public class MessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// The MessageRecievedEventArg constructor.
        /// </summary>
        /// <param name="message">The message (to be logged)</param>
        /// <param name="type">The message status</param>
        public MessageRecievedEventArgs(string message, MessageTypeEnum type)
        {
            Status = type;
            Message = message;
        }

        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public MessageTypeEnum Status { get; set; }     // The Message Type

        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; set; }             // The Message To Be Logged

        public static MessageTypeEnum ConvertType(EventLogEntryType type)
        {
            switch (type)
            {
                case (EventLogEntryType.Information): return MessageTypeEnum.INFO;
                case (EventLogEntryType.Warning): return MessageTypeEnum.WARNING;
                case (EventLogEntryType.FailureAudit): return MessageTypeEnum.FAIL;
            }
            return MessageTypeEnum.INFO;
        }
    }
}
