using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServieGUI.Model
{
    abstract class IModel
    {
        public virtual event EventHandler<SettingsEventArgs> changeInModel;
        protected virtual void InvokeEvent(SettingsEventArgs e)
        {
            Debug.WriteLine("before Imodel invoke");
           // Delegate[] d = changeInModel.GetInvocationList();
            //foreach (Delegate s in d) Debug.WriteLine(s.ToString());
            changeInModel?.Invoke(this, e);
            Debug.WriteLine("after Imodel invoke");
        }
        public abstract void MessageFromServer(object sender, CommandEventArgs args);
        public abstract void SendMessageToServer(CommandEnum commandID, string args);
    }
}
