using ImageServiceWeb.Models.Communication;
using ImageServiceWeb.Models.Config;
using ImageServiceWeb.Models.ImageWeb;
using ImageServiceWeb.Models.Logs;
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

        public HomeController()
        {
            client = new WebClient();
            client.Connect();
            imageWebModel = new ImageWebModel(client);
            configModel = new ConfigModel(client);
            logsModel = new LogsModel(client);
            
            //Session.Add["TcpConnection"] = client;
        }

        // GET: Home
        public ActionResult Index()
        {
            List<StudentInfo> students = new List<StudentInfo>(){
            new StudentInfo() { FirstName = "Liora", LastName = "Zaidner", StudentID = 32377 },
            new StudentInfo() { FirstName = "Lio", LastName = "Zaid", StudentID = 323 }
            };
            imageWebModel.Students = students;
            return View(imageWebModel);
        }

        public ActionResult Config()
        {
            return View(configModel);
        }

        public ActionResult Logs()
        {
            return View(logsModel);
        }
    }
}