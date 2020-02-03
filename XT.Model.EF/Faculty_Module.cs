namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Faculty_Module
    {
        public int Id { get; set; }

        public int Faculty_Id { get; set; }

        public int Module_Id { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Faculty Faculty { get; set; }
        public virtual Module Module { get; set; }
    }
}
