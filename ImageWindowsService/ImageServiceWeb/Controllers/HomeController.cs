using ImageServiceWeb.Models.Communication;
using ImageServiceWeb.Models.ImageWeb;
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

        public HomeController()
        {
            client = new WebClient();
            imageWebModel = new ImageWebModel(client);
            client.Connect();
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
            return View();
        }
    }
}