namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableUser_Company : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User_Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .ForeignKey("dbo.User_Profile", t => t.User_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Company_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Company", "User_Id", "dbo.User_Profile");
            DropForeignKey("dbo.User_Company", "Company_Id", "dbo.Company");
            DropIndex("dbo.User_Company", new[] { "Company_Id" });
            DropIndex("dbo.User_Company", new[] { "User_Id" });
            DropTable("dbo.User_Company");
        }
    }
}
