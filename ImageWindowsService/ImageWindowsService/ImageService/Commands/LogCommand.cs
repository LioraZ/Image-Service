using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    /// <summary>
    /// Class LogCommand.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class LogCommand : ICommand
    {
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
            string message = args[0];
            result = true;
            try
            {
                int messageType = int.Parse(args[1]);
                return message + ";" + messageType;
            }
            catch { }
            return message + ";" + ((int)MessageTypeEnum.INFO).ToString();
        }
    }
}
