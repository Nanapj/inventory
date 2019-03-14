namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F11 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NoteDetails", "Noted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NoteDetails", "Noted", c => c.String());
        }
    }
}
