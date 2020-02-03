namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account_Username = c.String(nullable: false, maxLength: 300),
                        Account_Password = c.String(nullable: false, maxLength: 300),
                        Account_Name = c.String(nullable: false, maxLength: 200),
                        Account_Avatar = c.String(maxLength: 100),
                        Account_Email = c.String(nullable: false, maxLength: 100),
                        HasSetPassword = c.Boolean(nullable: false),
                        User_Profile_Id = c.Int(nullable: false),
                        Account_ActiveKey = c.String(maxLength: 500),
                        Account_RecoverPasswordKey = c.String(maxLength: 500, unicode: false),
                        Account_RecoverPasswordExpired = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User_Profile", t => t.User_Profile_Id)
                .Index(t => t.User_Profile_Id);
            
            CreateTable(
                "dbo.User_Profile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Profile_Name = c.String(nullable: false, maxLength: 100),
                        User_Profile_Phone = c.String(maxLength: 20),
                        User_Profile_Email = c.String(nullable: false, maxLength: 100),
                        User_Type_Id = c.Int(nullable: false),
                        Role_Type_Id = c.Int(nullable: false),
                        User_Profile_Email_2 = c.String(maxLength: 100),
                        User_Profile_Avatar = c.String(maxLength: 100),
                        User_Profile_Facebook = c.String(maxLength: 50),
                        User_Profile_Viber = c.String(maxLength: 100),
                        User_Profile_Address = c.String(maxLength: 1000),
                        User_Profile_Gender = c.Int(),
                        User_Profile_Birthday = c.DateTime(storeType: "date"),
                        Status = c.Int(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role_Type", t => t.Role_Type_Id)
                .ForeignKey("dbo.User_Type", t => t.User_Type_Id)
                .Index(t => t.User_Type_Id)
                .Index(t => t.Role_Type_Id);
            
            CreateTable(
                "dbo.Role_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role_Type_Name = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Type_Name = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Profile", "User_Type_Id", "dbo.User_Type");
            DropForeignKey("dbo.User_Profile", "Role_Type_Id", "dbo.Role_Type");
            DropForeignKey("dbo.Account", "User_Profile_Id", "dbo.User_Profile");
            DropIndex("dbo.User_Profile", new[] { "Role_Type_Id" });
            DropIndex("dbo.User_Profile", new[] { "User_Type_Id" });
            DropIndex("dbo.Account", new[] { "User_Profile_Id" });
            DropTable("dbo.User_Type");
            DropTable("dbo.Role_Type");
            DropTable("dbo.User_Profile");
            DropTable("dbo.Account");
        }
    }
}
