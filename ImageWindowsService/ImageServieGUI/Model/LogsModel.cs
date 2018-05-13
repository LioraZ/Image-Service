using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageServieGUI.Model
{
    class LogsModel
    {
        public event EventHandler<MessageReceivedEventArgs> ReceivedLog;

        public void MessageFromServer(object sender, string message)
        {
            throw new NotImplementedException();
        }
    }
}
