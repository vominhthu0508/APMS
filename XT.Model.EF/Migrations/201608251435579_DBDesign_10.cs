namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student_FeePlan_Installment", "Amount_Planning", c => c.Int(nullable: false));
            AddColumn("dbo.Student_FeePlan_Installment", "Amount_Actual", c => c.Int(nullable: false));
            DropColumn("dbo.Student_FeePlan_Installment", "Actual_Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Student_FeePlan_Installment", "Actual_Amount", c => c.Int(nullable: false));
            DropColumn("dbo.Student_FeePlan_Installment", "Amount_Actual");
            DropColumn("dbo.Student_FeePlan_Installment", "Amount_Planning");
        }
    }
}
