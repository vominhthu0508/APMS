namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class_Module
    {
        public Class_Module()
        {
            Class_Module_Days = new HashSet<Class_Module_Day>();
            Class_Module_StudentExams = new HashSet<Class_Module_StudentExam>();
        }

        public int Id { get; set; }

        public int? Class_Id { get; set; }

        public int Module_Id { get; set; }

        public int Faculty_Id { get; set; }

        public int Resource_LT_Id { get; set; }

        public int Resource_TH_Id { get; set; }

        public int Resource_Exam_Id { get; set; }

        /// <summary>
        /// Tên class (Class.name nếu class != null hoặc ClassName nếu class == null – vd review HTML)
        /// </summary>
        public string Class_Module_Name { get; set; }

        /// <summary>
        /// [ClassDayEnum] Ca học (VD: 2,4,6 hoặc 3,5,7, CN)
        /// </summary>
        public int Class_Module_Day { get; set; }

        public DateTime Class_Module_Date_Start { get; set; }

        public DateTime Class_Module_Date_End { get; set; }

        public DateTime Class_Module_Date_Exam { get; set; }

        public float Class_Module_Hour_Start { get; set; }

        public float Class_Module_Hour_End { get; set; }

        public int Class_Module_DurationByDay { get; set; }

        public string Class_Module_Note { get; set; }

        public string Class_Module_RenderColor { get; set; }

        /// <summary>
        /// [ClassModuleStatusEnum] Dự tính/Đã học (Scheduled/Studying)
        /// </summary>
        public int Class_Module_Status { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Class Class { get; set; }

        public virtual Module Module { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual Resource Resource_LT { get; set; }

        public virtual Resource Resource_TH { get; set; }

        public virtual Resource Resource_Exam { get; set; }

        public virtual ICollection<Class_Module_Day> Class_Module_Days { get; set; }
        public virtual ICollection<Class_Module_StudentExam> Class_Module_StudentExams { get; set; }
    }
}
