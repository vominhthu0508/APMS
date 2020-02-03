namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTable_Faculty_Nickname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faculty", "FC_Nickname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faculty", "FC_Nickname");
        }
    }
}
