namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Diem cua SV trong tung mon hoc (Class_Module)
    /// </summary>
    public partial class Class_Module_StudentExam
    {
        public Class_Module_StudentExam()
        {
            Prizes = new HashSet<Prize>();
        }

        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int Class_Module_Id { get; set; }

        /// <summary>
        /// [StudentClassModuleStatusEnum] Trạng thái (Official/Guest)
        /// </summary>
        public int Student_Status { get; set; }

        public float Mark_LT { get; set; }
        public float Mark_TH { get; set; }
        public float Mark_LT_Percentage { get; set; }
        public float Mark_TH_Percentage { get; set; }
        /// <summary>
        /// Lần thi thứ mấy
        /// </summary>
        public int Exam_Count { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student Student { get; set; }
        public virtual Class_Module Class_Module { get; set; }

        public virtual ICollection<Prize> Prizes { get; set; }
    }
}
