using System;
using System.Diagnostics;
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
        #region Members
        private ImageServer imageServer;          // The Image Server
        private IImageServiceModel model;         // The Image Model
        private IImageController controller;      // The Commands Controller
        private ILoggingService logger;           // The Image Event Logger
        #endregion

        /// <summary>
        /// The ImageServer's constructor.
        /// </summary>
        /// <param name="args"></param> The proogram's arguments.
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

        /// <summary>
        /// The method ovverides the onStart method from serviceBase to start the service.
        /// </summary>
        /// <param name="args"></param> Args for the service.
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");

            logger = new LoggingService();
            model = new ImageServiceModel();
            controller = new ImageController(model);
            imageServer = new ImageServer(logger, controller);
            logger.MessageReceived += onMessageReceived;
            CreateHandlers();

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// The method overrides the onStop method from ServiceBase to stop the service.
        /// </summary>
        protected override void OnStop()
        {
            imageServer.SendCommand("Close Handler", "", null);
            logger.Log("In onStop", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// The method logs the given message to the EventLogger.
        /// </summary>
        /// <param name="sender"></param> The object the invoked the event that this method was registered to.
        /// <param name="args"></param> THe args for the event invoked.
        private void onMessageReceived(object sender, MessageRecievedEventArgs args)
        {
            eventLog1.WriteEntry(args.Message, logger.GetMessageType(args.Status));
        }

        /// <summary>
        /// The method creates directory handlers based on the directories received from app.config.
        /// </summary>
        private void CreateHandlers()
        {
            //imageServer.CreateHandler(ConfigurationManager.AppSettings["Handler"]);
            string handlerPaths = ConfigurationManager.AppSettings["Handler"];
            string[] paths = handlerPaths.Split(';');
            foreach (string path in paths) imageServer.CreateHandler(path);
        }

        /// <summary>
        /// The method sets the service status.
        /// </summary>
        /// <param name="handle"></param> The handler.
        /// <param name="serviceStatus"></param> The service status.
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// The method ovverides a method from EventLog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e) { }
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
