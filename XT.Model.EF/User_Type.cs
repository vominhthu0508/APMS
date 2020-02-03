namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// EmployeeType
    /// </summary>
    public partial class User_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User_Type()
        {
            User_Profiles = new HashSet<User_Profile>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string User_Type_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Profile> User_Profiles { get; set; }
    }
}
