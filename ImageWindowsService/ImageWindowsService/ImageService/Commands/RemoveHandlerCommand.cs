using ImageService.Commands;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    /// <summary>
    /// Class RemoveHandlerCommand.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    public class RemoveHandlerCommand : ICommand
    {
        public event EventHandler<string> RemoveHandler;
        
        /// <summary>
        /// The method will execute the given command with its arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
        /// The command's arguments.
        /// The result of the command success/failure.
        public string Execute(string[] args, out bool result)
        {
            if (GetCongigCommand.handlers.Contains(args[0])) GetCongigCommand.handlers.Remove(args[0]);
            RemoveHandler?.Invoke(this, args[0]);
            result = true;
            return args[0];
        }
    }
}
