namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CourseFamily", "CourseFamily_Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourseFamily", "CourseFamily_Year", c => c.DateTime(nullable: false));
        }
    }
}
