namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student_ClassHistory
    {
        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int Class_Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ChangeReason { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }
    }
}
