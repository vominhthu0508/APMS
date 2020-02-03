namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class_Module_Day
    {
        public Class_Module_Day()
        {
            Class_Module_Day_Students = new HashSet<Class_Module_Day_Student>();
        }

        public int Id { get; set; }

        public int Class_Module_Id { get; set; }

        //
        //public int Class_Module_Day_STT { get; set; }

        public DateTime Class_Module_Day_Date { get; set; }

        //public int Class_Module_Day_Hour_Start { get; set; }

        //public int Class_Module_Day_Hour_End { get; set; }

        /// <summary>
        /// [ClassModuleDayStatusEnum] Trạng thái (Studying, FC_Off, Center_Off)
        /// </summary>
        public int Class_Module_Day_Status { get; set; }

        public string Class_Module_Day_Note { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Class_Module Class_Module { get; set; }

        public virtual ICollection<Class_Module_Day_Student> Class_Module_Day_Students { get; set; }
    }
}
