namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class_Module_Day_Student
    {
        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int Class_Module_Day_Id { get; set; }

        /// <summary>
        /// [StudentClassModuleAttendanceEnum] Trạng thái (P/A/PA)
        /// </summary>
        public int Attendance_Status { get; set; }

        public string Attendance_Note { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student Student { get; set; }
        public virtual Class_Module_Day Class_Module_Day { get; set; }
    }
}
