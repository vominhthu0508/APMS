namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookOrder
    {
        public BookOrder()
        {
            BookOrder_Details = new HashSet<BookOrder_Detail>();
        }

        //key
        public int Id { get; set; }

        public int Company_Id { get; set; }

        //info
        public DateTime Indent_Date { get; set; }

        public int Indent_Number { get; set; }

        public string Center { get; set; }

        /// <summary>
        /// [IndentStatusEnum] Pending/Approved
        /// </summary>
        public int Indent_Status { get; set; }

        public string SAP_Customer_ID { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company Company { get; set; }

        public virtual ICollection<BookOrder_Detail> BookOrder_Details { get; set; }
    }
}
