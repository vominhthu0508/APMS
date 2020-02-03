namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlan", "FeePlan_Type", c => c.Int(nullable: false));
            DropColumn("dbo.FeePlan", "FeePlay_Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FeePlan", "FeePlay_Type", c => c.Int(nullable: false));
            DropColumn("dbo.FeePlan", "FeePlan_Type");
        }
    }
}
