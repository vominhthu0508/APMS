namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookOrder_Detail
    {
        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int BookOrder_Id { get; set; }

        public int Semester { get; set; }

        public string BookCode { get; set; }

        public int BookPrice { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student Student { get; set; }
        public virtual BookOrder BookOrder { get; set; }
    }
}
