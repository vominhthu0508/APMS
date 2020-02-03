namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseFamily", "CourseFamily_Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseFamily", "CourseFamily_Year");
        }
    }
}
