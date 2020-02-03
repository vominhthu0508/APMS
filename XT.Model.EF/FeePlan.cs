namespace XT.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeePlan
    {
        public FeePlan()
        {
            FeePlan_Details = new HashSet<FeePlan_Detail>();
            Student_FeePlans = new HashSet<Student_FeePlan>();
        }

        public int Id { get; set; }

        public int Company_Type_Id { get; set; }

        public string FeePlan_Name { get; set; }

        /// <summary>
        /// FeePlanTypeEnum (Semester/Lumpsum)
        /// </summary>
        public int FeePlan_Type { get; set; }

        /// <summary>
        /// Giá tiền (USD)
        /// </summary>
        public int FeePlan_Price { get; set; }

        /// <summary>
        /// Số lần đóng
        /// </summary>
        public int FeePlan_Count { get; set; }

        /// <summary>
        /// Số tháng học
        /// </summary>
        public int FeePlan_Months { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        //References
        public virtual Company_Type Company_Type { get; set; }

        public virtual ICollection<FeePlan_Detail> FeePlan_Details { get; set; }
        public virtual ICollection<Student_FeePlan> Student_FeePlans { get; set; }
    }
}
