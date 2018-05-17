using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class CommandEventArgs
    {
        public CommandEnum CommandID { get; set; }
        public string[] CommandArgs { get; set; }
    }
}
