namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeePlan_Detail
    {
        public FeePlan_Detail()
        {
            Student_FeePlan_Installments = new HashSet<Student_FeePlan_Installment>();
        }

        public int Id { get; set; }

        public int FeePlan_Id { get; set; }

        public int FeePlan_Index { get; set; }

        /// <summary>
        /// Giá từng đợt
        /// </summary>
        public int FeePlan_Amount { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual FeePlan FeePlan { get; set; }

        public virtual ICollection<Student_FeePlan_Installment> Student_FeePlan_Installments { get; set; }
    }
}
