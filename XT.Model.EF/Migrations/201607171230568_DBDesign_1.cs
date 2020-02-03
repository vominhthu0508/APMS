namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBDesign_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company_Type", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Company_Type", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Company", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Company", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.BookOrder", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.BookOrder", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.BookOrder_Detail", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Student", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Student", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Class", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Class", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Class_Module", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Class_Module", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Class_Module_Day", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Class_Module_Day", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Class_Module_Day_Student", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Class_Module_Day_Student", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Class_Module_StudentExam", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Class_Module_StudentExam", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prize", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Prize", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Faculty", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Faculty", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Faculty_Module", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Module", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Module", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.CourseFamily", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.CourseFamily", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Course", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Course", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Resource", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Resource", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Student_ClassHistory", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Student_ClassHistory", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Student_AcademicStatus", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Student_AcademicStatus", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Student_FeePlan", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Student_FeePlan", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.FeePlan", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.FeePlan", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.FeePlan_Detail", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.FeePlan_Detail", "Created_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Student_FeePlan_Installment", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Student_FeePlan_Installment", "Created_Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student_FeePlan_Installment", "Created_Date");
            DropColumn("dbo.Student_FeePlan_Installment", "Status");
            DropColumn("dbo.FeePlan_Detail", "Created_Date");
            DropColumn("dbo.FeePlan_Detail", "Status");
            DropColumn("dbo.FeePlan", "Created_Date");
            DropColumn("dbo.FeePlan", "Status");
            DropColumn("dbo.Student_FeePlan", "Created_Date");
            DropColumn("dbo.Student_FeePlan", "Status");
            DropColumn("dbo.Student_AcademicStatus", "Created_Date");
            DropColumn("dbo.Student_AcademicStatus", "Status");
            DropColumn("dbo.Student_ClassHistory", "Created_Date");
            DropColumn("dbo.Student_ClassHistory", "Status");
            DropColumn("dbo.Resource", "Created_Date");
            DropColumn("dbo.Resource", "Status");
            DropColumn("dbo.Course", "Created_Date");
            DropColumn("dbo.Course", "Status");
            DropColumn("dbo.CourseFamily", "Created_Date");
            DropColumn("dbo.CourseFamily", "Status");
            DropColumn("dbo.Module", "Created_Date");
            DropColumn("dbo.Module", "Status");
            DropColumn("dbo.Faculty_Module", "Created_Date");
            DropColumn("dbo.Faculty", "Created_Date");
            DropColumn("dbo.Faculty", "Status");
            DropColumn("dbo.Prize", "Created_Date");
            DropColumn("dbo.Prize", "Status");
            DropColumn("dbo.Class_Module_StudentExam", "Created_Date");
            DropColumn("dbo.Class_Module_StudentExam", "Status");
            DropColumn("dbo.Class_Module_Day_Student", "Created_Date");
            DropColumn("dbo.Class_Module_Day_Student", "Status");
            DropColumn("dbo.Class_Module_Day", "Created_Date");
            DropColumn("dbo.Class_Module_Day", "Status");
            DropColumn("dbo.Class_Module", "Created_Date");
            DropColumn("dbo.Class_Module", "Status");
            DropColumn("dbo.Class", "Created_Date");
            DropColumn("dbo.Class", "Status");
            DropColumn("dbo.Student", "Created_Date");
            DropColumn("dbo.Student", "Status");
            DropColumn("dbo.BookOrder_Detail", "Created_Date");
            DropColumn("dbo.BookOrder", "Created_Date");
            DropColumn("dbo.BookOrder", "Status");
            DropColumn("dbo.Company", "Created_Date");
            DropColumn("dbo.Company", "Status");
            DropColumn("dbo.Company_Type", "Created_Date");
            DropColumn("dbo.Company_Type", "Status");
        }
    }
}
