namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableStudent_AddEducation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "HighSchool", c => c.String());
            AddColumn("dbo.Student", "University", c => c.String());
            AddColumn("dbo.Student", "Company", c => c.String());
            AddColumn("dbo.Student", "Company_Address", c => c.String());
            AddColumn("dbo.Student", "Company_Position", c => c.String());
            AddColumn("dbo.Student", "Company_Salary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "Company_Salary");
            DropColumn("dbo.Student", "Company_Position");
            DropColumn("dbo.Student", "Company_Address");
            DropColumn("dbo.Student", "Company");
            DropColumn("dbo.Student", "University");
            DropColumn("dbo.Student", "HighSchool");
        }
    }
}
