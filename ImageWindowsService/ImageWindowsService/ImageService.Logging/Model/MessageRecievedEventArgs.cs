using System;

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

        public MessageTypeEnum Status { get; set; }     // The Message Type
        public string Message { get; set; }             // The Message To Be Logged
    }
}
