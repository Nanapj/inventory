namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F19 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NoteDetails", "Unit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NoteDetails", "Unit", c => c.String());
        }
    }
}
