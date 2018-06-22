using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageServiceWeb.Models.Communication;
using ImageServiceWeb.Models.Enums;
using Infrastructure.WebAppInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models.ImageWeb
{
    public class ImageWebModel
    {
        private IWebClient client;

        [DataType(DataType.Text)]
        [Display(Name = "Service Status: ")]
        public ServiceStatusEnum status { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Number Of Images: ")]
        public int NumPictures { get; set; }

        private List<StudentInfo> students = new List<StudentInfo>();
        public List<StudentInfo> Students { get; set; }
        

        public ImageWebModel(IWebClient webClient, string outputDir)
        {
            client = webClient;
            client.OnDataReceived += GetData;
            status = StatusConverter(client.isConnected());
            Students = GetStudentInfo();
           // client.SendCommand(CommandEnum.GetStudentsInfo);
            //client.SendCommand(CommandEnum.GetNumImages);
            NumPictures = GetNumImages(outputDir);

        }
        public void GetData(object sender, CommandEventArgs e)
        {
            if (e.CommandID == CommandEnum.GetStudentsInfo)
            {
                string jsonString = e.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StudentInfo>>(jsonString);
                Students = (List<StudentInfo>)obj;
            }
            if (e.CommandID == CommandEnum.GetNumImages)
            {
                string jsonString = e.CommandArgs[0];
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(jsonString);
                NumPictures = (int)obj;
            }
        }

        private ServiceStatusEnum StatusConverter(bool connected)
        {
            if (connected) return ServiceStatusEnum.RUNNING;
            return ServiceStatusEnum.INACTIVE;
        }

        private int GetNumImages(string outputDir)
        {
            int counter = 0;
            try
            {
                string[] directories = Directory.GetDirectories(outputDir + "\\Thumbnails");

                foreach (string directory in directories)
                {
                    string[] subDirectories = Directory.GetDirectories(directory);
                    foreach (string subDirectory in subDirectories)
                    {
                        string[] directoryFiles = Directory.GetFiles(subDirectory);
                        foreach (string filePath in directoryFiles)
                        {
                            string extension = Path.GetExtension(filePath);
                            if (extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".jpeg" 
                                || extension == ".gif" || extension == ".thumb") counter++;
                        }
                    }
                }
            }
            catch { }
            return counter;
        }

        private List<StudentInfo> GetStudentInfo()
        {
            List<StudentInfo> students = new List<StudentInfo>();
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Liora\Documents\ImageService\ImageWindowsService\ImageServiceWeb\Models\ImageWeb\StudentsInformation.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                StudentInfo studentInfo = new StudentInfo();
                string[] studentArr = line.Split('|');
                studentInfo.FirstName = studentArr[0];
                studentInfo.LastName = studentArr[1];
                try { studentInfo.StudentID = int.Parse(studentArr[2]); }
                catch { studentInfo.StudentID = 0; }
                students.Add(studentInfo);
            }

            file.Close();
            return students;
        }
    }
}