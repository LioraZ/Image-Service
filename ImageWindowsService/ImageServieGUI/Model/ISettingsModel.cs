using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using Infrastructure.Event;
using System;
using System.Diagnostics;

namespace ImageServieGUI.Model
{
    /// <summary>
    /// Class ISettingsModel.
    /// </summary>
    abstract class ISettingsModel
    {
        public virtual event EventHandler<string> OnHandlerRemoved;
        public virtual event EventHandler<SettingsEventArgs> changeInModel;
        /// <summary>
        /// Invokes the event.
        /// </summary>
        /// <param name="e">The <see cref="SettingsEventArgs"/> instance containing the event data.</param>
        protected virtual void InvokeEvent(SettingsEventArgs e)
        {
            Debug.WriteLine("before Imodel invoke");
           // Delegate[] d = changeInModel.GetInvocationList();
            //foreach (Delegate s in d) Debug.WriteLine(s.ToString());
            changeInModel?.Invoke(this, e);
            Debug.WriteLine("after Imodel invoke");
        }
        /// <summary>
        /// Messages from server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        public abstract void MessageFromServer(object sender, CommandEventArgs args);
        /// <summary>
        /// Sends the message to server.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        public abstract void SendMessageToServer(CommandEnum commandID, string args);
    }
}
