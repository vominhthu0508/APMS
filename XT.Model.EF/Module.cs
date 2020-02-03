namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Module
    {
        public Module()
        {
            Faculty_Modules = new HashSet<Faculty_Module>();
            Class_Modules = new HashSet<Class_Module>();
        }

        public int Id { get; set; }

        public int CourseFamily_Id { get; set; }

        public int Semester { get; set; }

        [Required]
        [StringLength(100)]
        public string Module_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Module_Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Module_Name_Portal { get; set; }

        [Required]
        [StringLength(100)]
        public string Semester_Name_Portal { get; set; }

        /// <summary>
        /// [ModuleTypeEnum] Loại môn (LT, TH, LT + TH)
        /// </summary>
        public int Module_Type { get; set; }

        /// <summary>
        /// [ModulePortalTypeEnum]: Portal, Extra
        /// </summary>
        public int Module_Portal_Type { get; set; }

        /// <summary>
        /// [ModuleExamTypeEnum]: WithTest, WithoutTest
        /// </summary>
        public int Module_Exam_Type { get; set; }

        public int Module_Max_LT { get; set; }

        public int Module_Max_TH { get; set; }

        public float Module_DurationByHour { get; set; }

        public int Module_DurationByDay { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual CourseFamily CourseFamily { get; set; }

        public virtual ICollection<Faculty_Module> Faculty_Modules { get; set; }
        public virtual ICollection<Class_Module> Class_Modules { get; set; }
    }
}
