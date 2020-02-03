namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableUser_Company_CreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User_Company", "Created_Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User_Company", "Created_Date");
        }
    }
}
