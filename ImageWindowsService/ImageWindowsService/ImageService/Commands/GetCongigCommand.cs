﻿using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    class GetCongigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            StringBuilder sb = new StringBuilder();
            string handlers = ConfigurationManager.AppSettings["Handler"];
        }
    }
}
