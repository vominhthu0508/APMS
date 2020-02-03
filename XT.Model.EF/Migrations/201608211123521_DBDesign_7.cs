namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Class", "Class_Hour_Start", c => c.Single(nullable: false));
            AlterColumn("dbo.Class", "Class_Hour_End", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Class", "Class_Hour_End", c => c.Int(nullable: false));
            AlterColumn("dbo.Class", "Class_Hour_Start", c => c.Int(nullable: false));
        }
    }
}
