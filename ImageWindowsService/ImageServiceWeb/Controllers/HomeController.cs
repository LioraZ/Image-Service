using ImageService.Logging.Model;
using ImageServiceWeb.Models.Communication;
using ImageServiceWeb.Models.Config;
using ImageServiceWeb.Models.Enums;
using ImageServiceWeb.Models.ImageWeb;
using ImageServiceWeb.Models.Logs;
using ImageServiceWeb.Models.PhotosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        List<StudentInfo> students = new List<StudentInfo>(){
            new StudentInfo() { FirstName = "Liora", LastName = "Zaidner", StudentID = 32377 },
            new StudentInfo() { FirstName = "Lio", LastName = "Zaid", StudentID = 323 }
        };
        private IWebClient client;
        private ImageWebModel imageWebModel;
        private ConfigModel configModel;
        private LogsModel logsModel;
        private PhotosModel photosModel;
        private static string removedHandler;

        public HomeController()
        {
            client = WebClient.GetInstance();
            //bool connected = client.Connect();
           // imageWebModel = new ImageWebModel(client);
            //imageWebModel.status = ServiceStatusEnum.RUNNING;//obvs need to check if connected
            //configModel = new ConfigModel(client);
            //logsModel = new LogsModel(client);
            //photosModel = new PhotosModel();
            
            //Session.Add["TcpConnection"] = client;
        }

        // GET: Home
        public ActionResult Index()
        {
            imageWebModel = new ImageWebModel(WebClient.GetInstance());
            /*List<StudentInfo> students = new List<StudentInfo>(){
            new StudentInfo() { FirstName = "Liora", LastName = "Zaidner", StudentID = 32377 },
            new StudentInfo() { FirstName = "Lio", LastName = "Zaid", StudentID = 323 }
            };
            imageWebModel.Students = students;
            imageWebModel.status = ServiceStatusEnum.RUNNING;*/
            return View(imageWebModel);
        }

        public ActionResult Config()
        {
            configModel = new ConfigModel(WebClient.GetInstance());
            return View(configModel);
        }

        public ActionResult Logs()
        {
            /*List<MessageRecievedEventArgs> logsList = new List<MessageRecievedEventArgs>()
            {
                new MessageRecievedEventArgs("Hello",MessageTypeEnum.INFO)
            };*/
            //logsModel.Logs = logsList;
            logsModel = new LogsModel(WebClient.GetInstance());
            return View(logsModel);
        }
        public ActionResult Photos()
        {
            photosModel = new PhotosModel();
            return View(photosModel);
        }

        public ActionResult Delete(string handler)
        {
           ViewBag.Handler = handler;
            /*configModel = new ConfigModel(WebClient.GetInstance());
            handler = Session["Handler"] as string;
            List<string> handlers = new List<string>(configModel.Handlers);
            handlers.Remove(handler);
            configModel.Handlers = handlers.ToArray();*/
            
            return View();
        }

        /// <exclude />
       // [HttpPost]
        public ActionResult OnDelete()
        {
            configModel = new ConfigModel(WebClient.GetInstance());
            //handler = Session["Handler"] as string;
            configModel.RemoveHandler(removedHandler);

            return RedirectToAction("Config", configModel);
        }

        [HttpPost]
        public ActionResult Photos(PhotoInfo photo)
        {
            return RedirectToAction("PhotosView", new { model = photo });
        }

        public ActionResult PhotosView()
        {
            return View();
        }
        public ActionResult PhotosDelete()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GetDeletePartial(string id)
        {
            //var deleteItem = db.Items.Find(id);  // I'm using 'Items' as a generic term for whatever item you have

            removedHandler = id;
            return PartialView("Delete", id);  // assumes your delete view is named "Delete"
        }

        [HttpPost]
        public ActionResult Logs(string type)
        {
            logsModel = new LogsModel(WebClient.GetInstance());
            if (MessageTypeEnum.INFO.ToString() == type || MessageTypeEnum.WARNING.ToString() == type
                || MessageTypeEnum.FAIL.ToString() == type)
            {
                List<MessageRecievedEventArgs> tempLogs = new List<MessageRecievedEventArgs>();
                foreach (MessageRecievedEventArgs log in logsModel.Logs)
                {
                    if (log.Status.ToString() == type) tempLogs.Add(log);
                }
                logsModel.Logs = tempLogs;
            }
            return View(logsModel);
        }
    }
}