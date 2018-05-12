﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageServieGUI.ViewModel
{
    class LogInfo
    {
        public string Name { get; set;}
        public int LogType { get; set; }

        public Brush LogTypeColor
        {
            get
            {
                switch (LogType)
                {
                    case 1:
                        return Brushes.LightGreen;
                    case 2:
                        return Brushes.Red;
                    case 3:
                        return Brushes.Yellow;
                }
                return Brushes.Transparent;
            }
        }
    }
}
