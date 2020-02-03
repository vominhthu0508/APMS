namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User_Profile")]
    public partial class User_Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User_Profile()
        {
            Accounts = new HashSet<Account>();
            User_Companies = new HashSet<User_Company>();
        }

        public int Id { get; set; }

        //Required
        [Required]
        [StringLength(100)]
        public string User_Profile_Name { get; set; }

        [StringLength(20)]
        public string User_Profile_Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string User_Profile_Email { get; set; }

        /// <summary>
        /// Employee_Type
        /// </summary>
        public int User_Type_Id { get; set; }
        /// <summary>
        /// Role_Type
        /// </summary>
        public int Role_Type_Id { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //Not Required

        //[StringLength(100)]
        //public string User_Profile_Email_2 { get; set; }

        [StringLength(100)]
        public string User_Profile_Avatar { get; set; }

        [StringLength(50)]
        public string User_Profile_Facebook { get; set; }

        //[StringLength(100)]
        //public string User_Profile_Viber { get; set; }

        [StringLength(1000)]
        public string User_Profile_Address { get; set; }

        public int? User_Profile_Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? User_Profile_Birthday { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References

        public virtual User_Type User_Type { get; set; }

        public virtual Role_Type Role_Type { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<User_Company> User_Companies { get; set; }
    }
}
