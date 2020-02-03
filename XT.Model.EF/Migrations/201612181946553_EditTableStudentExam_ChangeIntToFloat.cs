namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableStudentExam_ChangeIntToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_LT", c => c.Single(nullable: false));
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_TH", c => c.Single(nullable: false));
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_LT_Percentage", c => c.Single(nullable: false));
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_TH_Percentage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_TH_Percentage", c => c.Int(nullable: false));
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_LT_Percentage", c => c.Int(nullable: false));
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_TH", c => c.Int(nullable: false));
            AlterColumn("dbo.Class_Module_StudentExam", "Mark_LT", c => c.Int(nullable: false));
        }
    }
}
