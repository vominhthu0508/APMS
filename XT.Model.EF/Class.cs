namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class
    {
        public Class()
        {
            Class_Modules = new HashSet<Class_Module>();
            Students = new HashSet<Student>();
            Student_ClassHistories = new HashSet<Student_ClassHistory>();
        }

        public int Id { get; set; }

        public int Company_Id { get; set; }

        public int CourseFamily_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Class_Name { get; set; }

        public DateTime Class_Admission_Date { get; set; }

        public DateTime? Class_Completion_Date { get; set; }

        public DateTime? Class_Graduation_Date { get; set; }

        /// <summary>
        /// [ClassDayEnum] Ca học (VD: 2,4,6 hoặc 3,5,7, CN)
        /// </summary>
        public int Class_Day { get; set; }

        public float Class_Hour_Start { get; set; }

        public float Class_Hour_End { get; set; }

        /// <summary>
        /// [ClassStudyStatusEnum] Tình trạng lớp (Studying, Finish)
        /// </summary>
        public int Class_Studying_Status { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company Company { get; set; }

        public virtual CourseFamily CourseFamily { get; set; }

        public virtual ICollection<Class_Module> Class_Modules { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Student_ClassHistory> Student_ClassHistories { get; set; }
    }
}
