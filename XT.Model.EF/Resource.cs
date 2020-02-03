namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resource
    {
        public Resource()
        {
            Class_Module_LTs = new HashSet<Class_Module>();
            Class_Module_THs = new HashSet<Class_Module>();
            Class_Module_Exams = new HashSet<Class_Module>();
        }

        public int Id { get; set; }

        public int Company_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Resource_Name { get; set; }

        public int Resource_Capacity { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company Company { get; set; }

        public virtual ICollection<Class_Module> Class_Module_LTs { get; set; }
        public virtual ICollection<Class_Module> Class_Module_THs { get; set; }
        public virtual ICollection<Class_Module> Class_Module_Exams { get; set; }
    }
}
