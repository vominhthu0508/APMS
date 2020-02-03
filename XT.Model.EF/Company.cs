namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Company
    {
        public Company()
        {
            Faculties = new HashSet<Faculty>();
            Classes = new HashSet<Class>();
            BookOrders = new HashSet<BookOrder>();
            Resources = new HashSet<Resource>();
            User_Companies = new HashSet<User_Company>();
        }

        public int Id { get; set; }

        public int Company_Type_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Company_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Company_Name_Abbrev { get; set; }

        public string Company_Name_Portal { get; set; }

        [StringLength(512)]
        public string Company_Logo { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company_Type Company_Type { get; set; }

        public virtual ICollection<Faculty> Faculties { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<BookOrder> BookOrders { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<User_Company> User_Companies { get; set; }
    }
}
