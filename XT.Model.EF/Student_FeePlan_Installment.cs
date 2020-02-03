namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student_FeePlan_Installment
    {
        public int Id { get; set; }

        public int Student_FeePlan_Id { get; set; }

        public int FeePlan_Detail_Id { get; set; }//

        public DateTime Date_Planning { get; set; }//

        public DateTime? Date_Actual { get; set; }

        public DateTime? Date_Extend { get; set; }

        public int Extend_Count { get; set; }

        public int Amount_Planning { get; set; }//

        public int Amount_Actual { get; set; }

        /// <summary>
        /// InstallmentStatusEnum
        /// </summary>
        public int Installment_Status { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Student_FeePlan Student_FeePlan { get; set; }
        public virtual FeePlan_Detail FeePlan_Detail { get; set; }
    }
}
