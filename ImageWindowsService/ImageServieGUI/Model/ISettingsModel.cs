using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using Infrastructure.Event;
using System;
using System.Diagnostics;

namespace ImageServieGUI.Model
{
    abstract class ISettingsModel
    {
        public virtual event EventHandler<string> OnHandlerRemoved;
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
