using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageServiceWeb.Models.Communication;
using ImageServiceWeb.Models.Enums;
using Infrastructure.WebAppInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models.ImageWeb
{
    public class ImageWebModel
    {
        private IWebClient client;

        [DataType(DataType.Text)]
        [Display(Name = "Service Status")]
        public ServiceStatusEnum status { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Number Of Images")]
        public int numPictures { get; set; }

        private List<StudentInfo> students = new List<StudentInfo>();
        public List<StudentInfo> Students { get; set; }
        

        public ImageWebModel(IWebClient webClient)
        {
            //students = new List<StudentInfo>();
            //students.Add(new StudentInfo() { FirstName = "Liora", LastName = "Zaidner", StudentID = 32377 });
            //students.Add(new StudentInfo() { FirstName = "Lio", LastName = "Zaid", StudentID = 323 });
            status = ServiceStatusEnum.INACTIVE;
            client = webClient;
            client.OnDataReceived += GetData;
        }
        public void GetData(object sender, CommandEventArgs e)
        {
            if (e.CommandID == CommandEnum.GetStudentsInfo)
            {
                string jsonString = e.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StudentInfo>>(jsonString);
                students = (List<StudentInfo>)obj;
            }
           // if (e.CommandID == CommandEnum.)
           //add on disconnect server
        }
    }
}