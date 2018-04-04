using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Model;
using ImageService.Controller;
using ImageService.Logging;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {

        private System.Diagnostics.EventLog eventLog1;
        private int eventId = 1;

        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModel model;
        private IImageController controller;
        private ILoggingService logger;

        public ImageService(string[] args)
        {
            InitializeComponent();
            // ILogging loggingModel = new LoggingModel();
            string eventSourceName = "MySource";
            string logName = "MyNewLog";
            if (args.Count() > 0) { eventSourceName = args[0]; }
            if (args.Count() > 1) { logName = args[1]; }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            // LoggingModel.onMsgEvent += onMsg("Created Service");
        }

        protected override void OnStart(string[] args)
        {
            m_imageServer = new ImageServer();
            logger = new LoggingService();

            eventLog1.WriteEntry("In OnStart");
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop() { eventLog1.WriteEntry("In onStop."); }

        protected override void OnContinue() { eventLog1.WriteEntry("In OnContinue."); }

        public void onMessageReceived(string msg)
        {
            eventLog1.WriteEntry(msg, eventId++);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

   /* public partial class ImageService : ServiceBase
    {
        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModel model;
        private IImageController controller;
        private ILoggingService logger;
        private System.Diagnostics.EventLog eventLog;
        private int eventId = 1;
    }*/
}
