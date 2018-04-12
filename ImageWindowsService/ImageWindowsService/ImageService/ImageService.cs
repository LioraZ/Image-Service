using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Model;
using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Model;
using System.Configuration;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {

        private System.Diagnostics.EventLog eventLog1;
        private int eventId = 1;

        private ImageServer imageServer;          // The Image Server
        private IImageServiceModel model;
        private IImageController controller;
        private ILoggingService logger;

        public ImageService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            logger = new LoggingService();
            model = new ImageServiceModel();
            controller = new ImageController(model);
            imageServer = new ImageServer(logger, controller);
            logger.MessageReceived += onMessageReceived;
            CreateHandlers();

            eventLog1.WriteEntry("In OnStart");
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            imageServer.SendCommand("Close Server", "", null);
            logger.Log("In onStop", MessageTypeEnum.INFO);
        }

        protected override void OnContinue() { eventLog1.WriteEntry("In OnContinue."); }

        private void onMessageReceived(object sender, MessageRecievedEventArgs args)
        {
            eventLog1.WriteEntry(args.Message, logger.GetMessageType(args.Status)); //make second args system.diagnostics type
        }

        private void CreateHandlers()
        {
            imageServer.CreateHandler(ConfigurationManager.AppSettings["Handler"]);
            /*string handlerPaths = ConfigurationManager.AppSettings["Handler"];
            string[] paths = handlerPaths.Split(';');
            foreach (string path in paths)
            {
                imageServer.CreateHandler(path);
            }*/
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
}
