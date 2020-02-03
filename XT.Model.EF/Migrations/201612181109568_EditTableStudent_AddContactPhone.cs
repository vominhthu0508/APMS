namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableStudent_AddContactPhone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "Student_ContactPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "Student_ContactPhone");
        }
    }
}
