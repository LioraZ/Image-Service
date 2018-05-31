using ImageService.Commands;
using Infrastructure.WebAppInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Commands
{
    class GetStudentsInfoCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            string[] studentsArr = (ConfigurationManager.AppSettings["Students"]).Split(';');
            List<StudentInfo> studentInfos = new List<StudentInfo>();
            foreach (string student in studentsArr)
            {
                StudentInfo studentInfo = new StudentInfo();
                string[] studentArr = student.Split('|');
                studentInfo.FirstName = studentArr[0];
                studentInfo.LastName = studentArr[1];
                try { studentInfo.StudentID = int.Parse(studentArr[2]); }
                catch { studentInfo.StudentID = 0; }
                studentInfos.Add(studentInfo);
            }
            result = true;
            return Newtonsoft.Json.JsonConvert.SerializeObject(studentInfos); ;
        }
    }
}
