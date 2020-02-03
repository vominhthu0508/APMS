namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Account_Username { get; set; }

        [Required]
        [StringLength(300)]
        public string Account_Password { get; set; }

        [Required]
        [StringLength(200)]
        public string Account_Name { get; set; }

        [StringLength(100)]
        public string Account_Avatar { get; set; }

        [Required]
        [StringLength(100)]
        public string Account_Email { get; set; }

        public bool HasSetPassword { get; set; }

        /////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////
        //Private Info
        public int User_Profile_Id { get; set; }

        [StringLength(500)]
        public string Account_ActiveKey { get; set; }

        [StringLength(500)]
        public string Account_RecoverPasswordKey { get; set; }

        public DateTime? Account_RecoverPasswordExpired { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual User_Profile User_Profile { get; set; }
    }
}
