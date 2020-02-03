namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CourseFamily
    {
        public CourseFamily()
        {
            Modules = new HashSet<Module>();
            Classes = new HashSet<Class>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }

        public int Course_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseFamily_Name { get; set; }

        public int CourseFamily_Year { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Course Course { get; set; }

        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
