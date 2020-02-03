namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student_FeePlan
    {
        public Student_FeePlan()
        {
            Student_FeePlan_Installments = new HashSet<Student_FeePlan_Installment>();
        }

        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int FeePlan_Id { get; set; }

        /// <summary>
        /// Ngày bắt đầu FeePlan
        /// </summary>
        public DateTime FeePlan_StartDate { get; set; }

        public int Nominal_Course_Fee { get; set; }//= FeePlan.FeePlan_Price (lay tu FeePlan de luu tru sau nay lo FeePlan thay doi)

        public int Discount_Amount { get; set; }

        public int Actual_Course_Fee { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual FeePlan FeePlan { get; set; }
        public virtual Student Student { get; set; }

        public virtual ICollection<Student_FeePlan_Installment> Student_FeePlan_Installments { get; set; }
    }
}
