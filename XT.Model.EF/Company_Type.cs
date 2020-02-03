namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Company_Type
    {
        public Company_Type()
        {
            Companies = new HashSet<Company>();
            Courses = new HashSet<Course>();
            FeePlans = new HashSet<FeePlan>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Company_Type_Name { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<FeePlan> FeePlans { get; set; }
    }
}
