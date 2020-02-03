namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "Company_Name_Portal", c => c.String());
            AddColumn("dbo.BookOrder", "SAP_Customer_ID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookOrder", "SAP_Customer_ID");
            DropColumn("dbo.Company", "Company_Name_Portal");
        }
    }
}
