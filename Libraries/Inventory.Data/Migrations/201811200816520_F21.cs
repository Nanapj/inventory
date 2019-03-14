namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NoteDetails", "Noted", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NoteDetails", "Noted");
        }
    }
}
