namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Student_FeePlan_Installment", "Date_Actual", c => c.DateTime());
            AlterColumn("dbo.Student_FeePlan_Installment", "Date_Extend", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Student_FeePlan_Installment", "Date_Extend", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Student_FeePlan_Installment", "Date_Actual", c => c.DateTime(nullable: false));
        }
    }
}
