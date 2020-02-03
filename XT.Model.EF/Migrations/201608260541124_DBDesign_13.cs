namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student_FeePlan", "FeePlan_StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student_FeePlan", "FeePlan_StartDate");
        }
    }
}
