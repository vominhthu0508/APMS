namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Course
    {
        public Course()
        {
            CourseFamilies = new HashSet<CourseFamily>();
        }

        public int Id { get; set; }

        public int Company_Type_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Course_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Course_Code { get; set; }

        public int Course_Semester_Count { get; set; }

        public int? Parent_Course_Id { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company_Type Company_Type { get; set; }

        public virtual Course Parent_Course { get; set; }

        public virtual ICollection<CourseFamily> CourseFamilies { get; set; }
    }
}
