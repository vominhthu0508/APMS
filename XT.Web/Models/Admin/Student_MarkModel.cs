using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;

namespace XT.Web.Models
{
    public class Student_MarkModel
    {
        public string Module_Name { get; set; }
        public string Mark_Name { get; set; }
        public ModuleTypeEnum Module_Type { get; set; }
        public float Mark { get; set; }
        public int Max_Mark { get; set; }
        public float Rate { get; set; }
        public string Rate_Status { get; set; }
    }
}