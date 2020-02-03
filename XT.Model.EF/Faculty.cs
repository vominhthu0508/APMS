namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Faculty
    {
        public Faculty()
        {
            Faculty_Modules = new HashSet<Faculty_Module>();
            Class_Modules = new HashSet<Class_Module>();
        }

        public int Id { get; set; }

        public int Company_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FC_Name { get; set; }

        public string FC_Nickname { get; set; }

        /// <summary>
        /// [FacultyTypeEnum] Loai FC (Full, Part)
        /// </summary>
        public int FC_Type { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //Contact Info

        public int FC_Gender { get; set; }

        public string FC_Phone { get; set; }

        public string FC_Email { get; set; }

        public string FC_Address { get; set; }

        public string FC_Address_Company { get; set; }

        public DateTime FC_Birthday { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //Other

        public long FC_Salary { get; set; }

        public int FC_WorkingHour { get; set; }

        public string FC_CMND { get; set; }

        public string FC_CMND_NoiCap { get; set; }

        public DateTime FC_CMND_NgayCap { get; set; }

        public string FC_MST { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company Company { get; set; }

        public virtual ICollection<Faculty_Module> Faculty_Modules { get; set; }
        public virtual ICollection<Class_Module> Class_Modules { get; set; }
    }
}
