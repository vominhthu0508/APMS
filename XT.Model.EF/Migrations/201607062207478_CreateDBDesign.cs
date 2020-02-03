namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDBDesign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Type_Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Type_Id = c.Int(nullable: false),
                        Company_Name = c.String(nullable: false, maxLength: 100),
                        Company_Name_Abbrev = c.String(nullable: false, maxLength: 100),
                        Company_Logo = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Type", t => t.Company_Type_Id)
                .Index(t => t.Company_Type_Id);
            
            CreateTable(
                "dbo.BookOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Id = c.Int(nullable: false),
                        Indent_Date = c.DateTime(nullable: false),
                        Indent_Number = c.Int(nullable: false),
                        Center = c.String(),
                        Indent_Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.BookOrder_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        BookOrder_Id = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        BookCode = c.String(),
                        BookPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .ForeignKey("dbo.BookOrder", t => t.BookOrder_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.BookOrder_Id);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseFamily_Id = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                        Student_EnrollNumber = c.String(),
                        Student_FirstName = c.String(),
                        Student_LastName = c.String(),
                        Student_Gender = c.Int(nullable: false),
                        Student_Birthday = c.DateTime(nullable: false),
                        Student_Avatar = c.String(),
                        Student_MobilePhone = c.String(),
                        Student_HomePhone = c.String(),
                        Student_Email = c.String(),
                        Student_Facebook = c.String(),
                        Student_Address = c.String(),
                        Student_ContactAddress = c.String(),
                        Student_City = c.String(),
                        Student_District = c.String(),
                        Student_Sponsor = c.String(),
                        Student_Sponsor_Relation = c.String(),
                        Student_Sponsor_Address = c.String(),
                        Student_Application_Date = c.DateTime(nullable: false),
                        Student_Application_CS = c.String(),
                        Student_Application_Documents = c.String(),
                        Student_Status = c.Int(nullable: false),
                        Student_Status_Date = c.DateTime(nullable: false),
                        Student_Promotion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseFamily", t => t.CourseFamily_Id)
                .ForeignKey("dbo.Class", t => t.Class_Id)
                .Index(t => t.CourseFamily_Id)
                .Index(t => t.Class_Id);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Id = c.Int(nullable: false),
                        CourseFamily_Id = c.Int(nullable: false),
                        Class_Name = c.String(nullable: false, maxLength: 100),
                        Class_Admission_Date = c.DateTime(nullable: false),
                        Class_Completion_Date = c.DateTime(),
                        Class_Graduation_Date = c.DateTime(),
                        Class_Day = c.Int(nullable: false),
                        Class_Hour_Start = c.Int(nullable: false),
                        Class_Hour_End = c.Int(nullable: false),
                        Class_Studying_Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseFamily", t => t.CourseFamily_Id)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .Index(t => t.Company_Id)
                .Index(t => t.CourseFamily_Id);
            
            CreateTable(
                "dbo.Class_Module",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Class_Id = c.Int(nullable: false),
                        Module_Id = c.Int(nullable: false),
                        Faculty_Id = c.Int(nullable: false),
                        Resource_LT_Id = c.Int(nullable: false),
                        Resource_TH_Id = c.Int(nullable: false),
                        Resource_Exam_Id = c.Int(nullable: false),
                        Class_Module_Name = c.String(),
                        Class_Module_Date_Start = c.DateTime(nullable: false),
                        Class_Module_Date_End = c.DateTime(nullable: false),
                        Class_Module_Date_Exam = c.DateTime(nullable: false),
                        Class_Module_Hour_Start = c.Int(nullable: false),
                        Class_Module_Hour_End = c.Int(nullable: false),
                        Class_Module_DurationByDay = c.Int(nullable: false),
                        Class_Module_Note = c.String(),
                        Class_Module_RenderColor = c.String(),
                        Class_Module_Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Faculty", t => t.Faculty_Id)
                .ForeignKey("dbo.Module", t => t.Module_Id)
                .ForeignKey("dbo.Resource", t => t.Resource_Exam_Id)
                .ForeignKey("dbo.Resource", t => t.Resource_LT_Id)
                .ForeignKey("dbo.Resource", t => t.Resource_TH_Id)
                .ForeignKey("dbo.Class", t => t.Class_Id)
                .Index(t => t.Class_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.Faculty_Id)
                .Index(t => t.Resource_LT_Id)
                .Index(t => t.Resource_TH_Id)
                .Index(t => t.Resource_Exam_Id);
            
            CreateTable(
                "dbo.Class_Module_Day",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Class_Module_Id = c.Int(nullable: false),
                        Class_Module_Day_STT = c.Int(nullable: false),
                        Class_Module_Day_Date = c.DateTime(nullable: false),
                        Class_Module_Day_Hour_Start = c.Int(nullable: false),
                        Class_Module_Day_Hour_End = c.Int(nullable: false),
                        Class_Module_Day_Status = c.Int(nullable: false),
                        Class_Module_Day_Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class_Module", t => t.Class_Module_Id)
                .Index(t => t.Class_Module_Id);
            
            CreateTable(
                "dbo.Class_Module_Day_Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Class_Module_Day_Id = c.Int(nullable: false),
                        Attendance_Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class_Module_Day", t => t.Class_Module_Day_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.Class_Module_Day_Id);
            
            CreateTable(
                "dbo.Class_Module_StudentExam",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Class_Module_Id = c.Int(nullable: false),
                        Student_Status = c.Int(nullable: false),
                        Mark_LT = c.Int(nullable: false),
                        Mark_TH = c.Int(nullable: false),
                        Mark_LT_Percentage = c.Int(nullable: false),
                        Mark_TH_Percentage = c.Int(nullable: false),
                        Exam_Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class_Module", t => t.Class_Module_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.Class_Module_Id);
            
            CreateTable(
                "dbo.Prize",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Exam_Id = c.Int(),
                        Title = c.String(),
                        Prize_Date = c.DateTime(nullable: false),
                        Prize_Type = c.Int(nullable: false),
                        Prize_Semester = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class_Module_StudentExam", t => t.Exam_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.Exam_Id);
            
            CreateTable(
                "dbo.Faculty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Id = c.Int(nullable: false),
                        FC_Name = c.String(nullable: false, maxLength: 100),
                        FC_Type = c.Int(nullable: false),
                        FC_Gender = c.Int(nullable: false),
                        FC_Phone = c.String(),
                        FC_Email = c.String(),
                        FC_Address = c.String(),
                        FC_Address_Company = c.String(),
                        FC_Birthday = c.DateTime(nullable: false),
                        FC_Salary = c.Long(nullable: false),
                        FC_WorkingHour = c.Int(nullable: false),
                        FC_CMND = c.String(),
                        FC_CMND_NoiCap = c.String(),
                        FC_CMND_NgayCap = c.DateTime(nullable: false),
                        FC_MST = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Faculty_Module",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Faculty_Id = c.Int(nullable: false),
                        Module_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Module", t => t.Module_Id)
                .ForeignKey("dbo.Faculty", t => t.Faculty_Id)
                .Index(t => t.Faculty_Id)
                .Index(t => t.Module_Id);
            
            CreateTable(
                "dbo.Module",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseFamily_Id = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        Module_Name = c.String(nullable: false, maxLength: 100),
                        Module_Code = c.String(nullable: false, maxLength: 100),
                        Module_Type = c.Int(nullable: false),
                        Module_Max_LT = c.Int(nullable: false),
                        Module_Max_TH = c.Int(nullable: false),
                        Module_DurationByHour = c.Int(nullable: false),
                        Module_DurationByDay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseFamily", t => t.CourseFamily_Id)
                .Index(t => t.CourseFamily_Id);
            
            CreateTable(
                "dbo.CourseFamily",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Course_Id = c.Int(nullable: false),
                        CourseFamily_Name = c.String(nullable: false, maxLength: 100),
                        CourseFamily_Year = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Course", t => t.Course_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Type_Id = c.Int(nullable: false),
                        Course_Name = c.String(nullable: false, maxLength: 100),
                        Course_Code = c.String(nullable: false, maxLength: 100),
                        Course_Semester_Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Type", t => t.Company_Type_Id)
                .Index(t => t.Company_Type_Id);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Id = c.Int(nullable: false),
                        Resource_Name = c.String(nullable: false, maxLength: 100),
                        Resource_Capacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Student_ClassHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        ChangeReason = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.Class_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.Class_Id);
            
            CreateTable(
                "dbo.Student_AcademicStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Student_Status = c.Int(nullable: false),
                        Student_Status_Date = c.DateTime(nullable: false),
                        Student_Status_Note = c.String(),
                        Student_FU_Date = c.DateTime(),
                        Student_FU_Amount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Student_FeePlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        FeePlan_Id = c.Int(nullable: false),
                        Nominal_Course_Fee = c.Int(nullable: false),
                        Discount_Amount = c.Int(nullable: false),
                        Actual_Course_Fee = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeePlan", t => t.FeePlan_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.FeePlan_Id);
            
            CreateTable(
                "dbo.FeePlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company_Type_Id = c.Int(nullable: false),
                        FeePlan_Name = c.String(),
                        FeePlay_Type = c.Int(nullable: false),
                        FeePlan_Price = c.Int(nullable: false),
                        FeePlan_Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company_Type", t => t.Company_Type_Id)
                .Index(t => t.Company_Type_Id);
            
            CreateTable(
                "dbo.FeePlan_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeePlan_Id = c.Int(nullable: false),
                        FeePlan_Index = c.Int(nullable: false),
                        FeePlan_Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeePlan", t => t.FeePlan_Id)
                .Index(t => t.FeePlan_Id);
            
            CreateTable(
                "dbo.Student_FeePlan_Installment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_FeePlan_Id = c.Int(nullable: false),
                        FeePlan_Detail_Id = c.Int(nullable: false),
                        Date_Planning = c.DateTime(nullable: false),
                        Date_Actual = c.DateTime(nullable: false),
                        Date_Extend = c.DateTime(nullable: false),
                        Extend_Count = c.Int(nullable: false),
                        Actual_Amount = c.Int(nullable: false),
                        Installment_Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeePlan_Detail", t => t.FeePlan_Detail_Id)
                .ForeignKey("dbo.Student_FeePlan", t => t.Student_FeePlan_Id)
                .Index(t => t.Student_FeePlan_Id)
                .Index(t => t.FeePlan_Detail_Id);
            
            DropColumn("dbo.User_Profile", "User_Profile_Email_2");
            DropColumn("dbo.User_Profile", "User_Profile_Viber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User_Profile", "User_Profile_Viber", c => c.String(maxLength: 100));
            AddColumn("dbo.User_Profile", "User_Profile_Email_2", c => c.String(maxLength: 100));
            DropForeignKey("dbo.FeePlan", "Company_Type_Id", "dbo.Company_Type");
            DropForeignKey("dbo.Course", "Company_Type_Id", "dbo.Company_Type");
            DropForeignKey("dbo.Company", "Company_Type_Id", "dbo.Company_Type");
            DropForeignKey("dbo.Resource", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.Faculty", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.Class", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.BookOrder", "Company_Id", "dbo.Company");
            DropForeignKey("dbo.BookOrder_Detail", "BookOrder_Id", "dbo.BookOrder");
            DropForeignKey("dbo.Student_FeePlan", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Student_FeePlan_Installment", "Student_FeePlan_Id", "dbo.Student_FeePlan");
            DropForeignKey("dbo.Student_FeePlan", "FeePlan_Id", "dbo.FeePlan");
            DropForeignKey("dbo.FeePlan_Detail", "FeePlan_Id", "dbo.FeePlan");
            DropForeignKey("dbo.Student_FeePlan_Installment", "FeePlan_Detail_Id", "dbo.FeePlan_Detail");
            DropForeignKey("dbo.Student_ClassHistory", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Student_AcademicStatus", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Prize", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Class_Module_StudentExam", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Class_Module_Day_Student", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Student", "Class_Id", "dbo.Class");
            DropForeignKey("dbo.Student_ClassHistory", "Class_Id", "dbo.Class");
            DropForeignKey("dbo.Class_Module", "Class_Id", "dbo.Class");
            DropForeignKey("dbo.Class_Module", "Resource_TH_Id", "dbo.Resource");
            DropForeignKey("dbo.Class_Module", "Resource_LT_Id", "dbo.Resource");
            DropForeignKey("dbo.Class_Module", "Resource_Exam_Id", "dbo.Resource");
            DropForeignKey("dbo.Faculty_Module", "Faculty_Id", "dbo.Faculty");
            DropForeignKey("dbo.Faculty_Module", "Module_Id", "dbo.Module");
            DropForeignKey("dbo.Student", "CourseFamily_Id", "dbo.CourseFamily");
            DropForeignKey("dbo.Module", "CourseFamily_Id", "dbo.CourseFamily");
            DropForeignKey("dbo.CourseFamily", "Course_Id", "dbo.Course");
            DropForeignKey("dbo.Class", "CourseFamily_Id", "dbo.CourseFamily");
            DropForeignKey("dbo.Class_Module", "Module_Id", "dbo.Module");
            DropForeignKey("dbo.Class_Module", "Faculty_Id", "dbo.Faculty");
            DropForeignKey("dbo.Class_Module_StudentExam", "Class_Module_Id", "dbo.Class_Module");
            DropForeignKey("dbo.Prize", "Exam_Id", "dbo.Class_Module_StudentExam");
            DropForeignKey("dbo.Class_Module_Day", "Class_Module_Id", "dbo.Class_Module");
            DropForeignKey("dbo.Class_Module_Day_Student", "Class_Module_Day_Id", "dbo.Class_Module_Day");
            DropForeignKey("dbo.BookOrder_Detail", "Student_Id", "dbo.Student");
            DropIndex("dbo.Student_FeePlan_Installment", new[] { "FeePlan_Detail_Id" });
            DropIndex("dbo.Student_FeePlan_Installment", new[] { "Student_FeePlan_Id" });
            DropIndex("dbo.FeePlan_Detail", new[] { "FeePlan_Id" });
            DropIndex("dbo.FeePlan", new[] { "Company_Type_Id" });
            DropIndex("dbo.Student_FeePlan", new[] { "FeePlan_Id" });
            DropIndex("dbo.Student_FeePlan", new[] { "Student_Id" });
            DropIndex("dbo.Student_AcademicStatus", new[] { "Student_Id" });
            DropIndex("dbo.Student_ClassHistory", new[] { "Class_Id" });
            DropIndex("dbo.Student_ClassHistory", new[] { "Student_Id" });
            DropIndex("dbo.Resource", new[] { "Company_Id" });
            DropIndex("dbo.Course", new[] { "Company_Type_Id" });
            DropIndex("dbo.CourseFamily", new[] { "Course_Id" });
            DropIndex("dbo.Module", new[] { "CourseFamily_Id" });
            DropIndex("dbo.Faculty_Module", new[] { "Module_Id" });
            DropIndex("dbo.Faculty_Module", new[] { "Faculty_Id" });
            DropIndex("dbo.Faculty", new[] { "Company_Id" });
            DropIndex("dbo.Prize", new[] { "Exam_Id" });
            DropIndex("dbo.Prize", new[] { "Student_Id" });
            DropIndex("dbo.Class_Module_StudentExam", new[] { "Class_Module_Id" });
            DropIndex("dbo.Class_Module_StudentExam", new[] { "Student_Id" });
            DropIndex("dbo.Class_Module_Day_Student", new[] { "Class_Module_Day_Id" });
            DropIndex("dbo.Class_Module_Day_Student", new[] { "Student_Id" });
            DropIndex("dbo.Class_Module_Day", new[] { "Class_Module_Id" });
            DropIndex("dbo.Class_Module", new[] { "Resource_Exam_Id" });
            DropIndex("dbo.Class_Module", new[] { "Resource_TH_Id" });
            DropIndex("dbo.Class_Module", new[] { "Resource_LT_Id" });
            DropIndex("dbo.Class_Module", new[] { "Faculty_Id" });
            DropIndex("dbo.Class_Module", new[] { "Module_Id" });
            DropIndex("dbo.Class_Module", new[] { "Class_Id" });
            DropIndex("dbo.Class", new[] { "CourseFamily_Id" });
            DropIndex("dbo.Class", new[] { "Company_Id" });
            DropIndex("dbo.Student", new[] { "Class_Id" });
            DropIndex("dbo.Student", new[] { "CourseFamily_Id" });
            DropIndex("dbo.BookOrder_Detail", new[] { "BookOrder_Id" });
            DropIndex("dbo.BookOrder_Detail", new[] { "Student_Id" });
            DropIndex("dbo.BookOrder", new[] { "Company_Id" });
            DropIndex("dbo.Company", new[] { "Company_Type_Id" });
            DropTable("dbo.Student_FeePlan_Installment");
            DropTable("dbo.FeePlan_Detail");
            DropTable("dbo.FeePlan");
            DropTable("dbo.Student_FeePlan");
            DropTable("dbo.Student_AcademicStatus");
            DropTable("dbo.Student_ClassHistory");
            DropTable("dbo.Resource");
            DropTable("dbo.Course");
            DropTable("dbo.CourseFamily");
            DropTable("dbo.Module");
            DropTable("dbo.Faculty_Module");
            DropTable("dbo.Faculty");
            DropTable("dbo.Prize");
            DropTable("dbo.Class_Module_StudentExam");
            DropTable("dbo.Class_Module_Day_Student");
            DropTable("dbo.Class_Module_Day");
            DropTable("dbo.Class_Module");
            DropTable("dbo.Class");
            DropTable("dbo.Student");
            DropTable("dbo.BookOrder_Detail");
            DropTable("dbo.BookOrder");
            DropTable("dbo.Company");
            DropTable("dbo.Company_Type");
        }
    }
}
