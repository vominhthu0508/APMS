namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Class_Module", new[] { "Class_Id" });
            AlterColumn("dbo.Class_Module", "Class_Id", c => c.Int());
            CreateIndex("dbo.Class_Module", "Class_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Class_Module", new[] { "Class_Id" });
            AlterColumn("dbo.Class_Module", "Class_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Class_Module", "Class_Id");
        }
    }
}
