
using ImageService.Logging.Model;
using System;
using System.IO;
using System.Collections.Generic;
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
            using (FileStream fs = File.Create("logsDoc.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(message);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            MessageReceived?.Invoke(this, new MessageRecievedEventArgs(message, type));
           
        }
    }
}
