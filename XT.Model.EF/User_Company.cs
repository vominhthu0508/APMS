namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Company
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Company_Id { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual User_Profile User_Profile { get; set; }
        public virtual Company Company { get; set; }
    }
}
