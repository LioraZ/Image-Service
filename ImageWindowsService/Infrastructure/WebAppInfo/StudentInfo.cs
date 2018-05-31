using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infrastructure.WebAppInfo
{
    public class StudentInfo
    {
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Student ID")]
        public int StudentID { get; set; }
    }
}