namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NoteDetails", "Unit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NoteDetails", "Unit");
        }
    }
}
