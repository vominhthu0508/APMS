namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTable_Module_DurationByHour : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Module", "Module_DurationByHour", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Module", "Module_DurationByHour", c => c.Int(nullable: false));
        }
    }
}
