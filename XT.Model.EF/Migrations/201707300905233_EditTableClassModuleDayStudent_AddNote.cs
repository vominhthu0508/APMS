namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableClassModuleDayStudent_AddNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Class_Module_Day_Student", "Attendance_Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Class_Module_Day_Student", "Attendance_Note");
        }
    }
}
