namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student_AcademicStatus
    {
        public int Id { get; set; }

        public int Student_Id { get; set; }

        /// <summary>
        /// [StudentAcademicStatusEnum] Trạng thái (studying, bảo lưu, drop out, note)
        /// </summary>
        public int Student_Status { get; set; }

        public DateTime Student_Status_Date { get; set; }

        public string Student_Status_Note { get; set; }

        public DateTime? Student_FU_Date { get; set; }

        public long Student_FU_Amount { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student Student { get; set; }
    }
}
