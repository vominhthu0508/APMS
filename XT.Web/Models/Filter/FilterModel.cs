using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;

namespace XT.Web.Models
{
    public class FilterModel_Base
    {
        public int? page;
        public int? page_size;
        public int pageChange;
        public string entity;
        public string Model_Name = "";
        public string sort_target = "";
        public bool sort_rank = false;
    }

    public class FilterModel_Class_Module : FilterModel_Base
    {
        public DateTime? Start_Date;
        public DateTime? End_Date;
        public string class_id = "";
        public int Class_Module_Status = 0;
        public int Semester = 0;
        //int Module_Id = 0,
        public int Id_Class = 0;
        public int Company_Id = 0;
        public int Class_Module_Day = 0;
        public int Faculty_Id = 0;
    }
}