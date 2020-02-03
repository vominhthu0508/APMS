namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlan", "FeePlan_Months", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlan", "FeePlan_Months");
        }
    }
}
