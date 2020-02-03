namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prize
    {
        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int? Exam_Id { get; set; }

        public string Title { get; set; }

        public DateTime Prize_Date { get; set; }

        /// <summary>
        /// [PrizeTypeEnum] Loại (môn/kỳ)
        /// </summary>
        public int Prize_Type { get; set; }

        public int Prize_Semester { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student Student { get; set; }
        public virtual Class_Module_StudentExam Exam { get; set; }
    }
}
