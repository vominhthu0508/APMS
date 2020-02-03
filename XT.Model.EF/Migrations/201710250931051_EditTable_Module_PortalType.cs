namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTable_Module_PortalType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Module", "Module_Portal_Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Module", "Module_Portal_Type");
        }
    }
}
