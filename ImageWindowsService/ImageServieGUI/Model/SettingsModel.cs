﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServieGUI.Communication;
using ImageService.Infrastructure.Enums;
using System.Diagnostics;
using ImageServieGUI.ViewModel;

namespace ImageServieGUI.Model
{
    class SettingsModel : IModel
    {
        /*private string outputDir;
        private string sourceName;
        private string logName;
        private int thumbnailSize;*/
        //private List<string> handlers;
        public override event EventHandler<SettingsEventArgs> changeInModel;

        /*public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }*/

        public SettingsModel()
        {
            CLient client = CLient.GetInstance();
            client.MessageReceived += MessageFromServer;
            client.SendMessageToServer(CommandEnum.GetConfigCommand, "");
        }

        private SettingsEventArgs ParseSettingsFromString(string settings)
        {
            SettingsEventArgs e = new SettingsEventArgs();
            Debug.WriteLine("Starting to parse settings");
            string[] splitSettings = settings.Split('|');
            e.OutputDir = splitSettings[0];
            e.SourceName = splitSettings[1];
            e.LogName = splitSettings[2];
            e.ThumbnailSize = int.Parse(splitSettings[3]);
            e.Handlers = splitSettings[4].Split(';'); ;
            Debug.WriteLine(e.OutputDir + e.SourceName + e.LogName + e.ThumbnailSize);
            return e;
        } 
        
        public override void MessageFromServer(object sender, string message)
        {
            CLient client = (CLient)sender;
            string settings = message.Substring(1);
            SettingsEventArgs e = ParseSettingsFromString(settings);
            changeInModel?.Invoke(this, e);
        }

        public override void SendMessageToServer(CommandEnum commandID, string args)
        {
            CLient client = CLient.GetInstance();
            client.SendMessageToServer(commandID, args);
        }
    }
}
