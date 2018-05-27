using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Event
{
    /// <summary>
    /// Class CommandEventArgs.
    /// </summary>
    public class CommandEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEventArgs"/> class.
        /// </summary>
        public CommandEventArgs() { }
        /// <summary>
        /// Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public CommandEnum CommandID { get; set; }
        /// <summary>
        /// Gets or sets the command arguments.
        /// </summary>
        /// <value>The command arguments.</value>
        public string[] CommandArgs { get; set; }
    }
}
