namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_8 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Class_Module_Day", "Class_Module_Day_STT");
            DropColumn("dbo.Class_Module_Day", "Class_Module_Day_Hour_Start");
            DropColumn("dbo.Class_Module_Day", "Class_Module_Day_Hour_End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Class_Module_Day", "Class_Module_Day_Hour_End", c => c.Int(nullable: false));
            AddColumn("dbo.Class_Module_Day", "Class_Module_Day_Hour_Start", c => c.Int(nullable: false));
            AddColumn("dbo.Class_Module_Day", "Class_Module_Day_STT", c => c.Int(nullable: false));
        }
    }
}
