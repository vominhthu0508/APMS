namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Class_Module", "Class_Module_Day", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Class_Module", "Class_Module_Day");
        }
    }
}
