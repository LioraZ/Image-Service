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
       // private IWebClient client;
        private ImageWebModel imageWebModel;
        private ConfigModel configModel;
        private LogsModel logsModel;
        private PhotosModel photosModel;
        private static string removedHandler;
        private static string outputDir;
        private static string removedPhoto;

        public HomeController()
        {
            IWebClient client = WebClient.GetInstance();
            if (client.isConnected() && outputDir == null)
            {
                configModel = new ConfigModel(client);
                outputDir = configModel.OutputDir;
            }
        }

        // GET: Home
        public ActionResult Index()
        {
            imageWebModel = new ImageWebModel(WebClient.GetInstance(), outputDir);
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
            photosModel = new PhotosModel(outputDir);
            return View(photosModel);
        }

        public ActionResult Delete(string handler)
        {
            ViewBag.Handler = handler;
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
        public ActionResult OnDeletePhoto()
        {
            photosModel = new PhotosModel(outputDir);
            photosModel.RemovePhoto(removedPhoto);
            return RedirectToAction("Photos", photosModel);
        }

        [HttpPost]
        public ActionResult Photos(PhotoInfo photo)
        {
            return RedirectToAction("PhotosView", new { model = photo });
        }


        /// <summary>
        /// Photoses the view.
        /// </summary>
        /// <returns>PartialViewResult.</returns>
        public PartialViewResult PhotosDelete(string id)
        {
            removedPhoto = id;
            photosModel = new PhotosModel(outputDir);
            return PartialView("PhotosDelete", id); //check that is not null
        }

    
        public ActionResult PhotosView(string id)
        {
            photosModel = new PhotosModel(outputDir);
            PhotoInfo p = photosModel.GetPhotoInfo(id);
            return View(p); //check that is not null
        }

        [HttpGet]
        public PartialViewResult GetDeletePartial(string id)
        {
            removedHandler = id;
            return PartialView("Delete", id); 
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

        public ActionResult ViewPhoto(string photo)
        {
            photosModel = new PhotosModel(outputDir);
            PhotoInfo p = photosModel.GetPhotoInfo(photo);
            return View(p);
        }
    }
}