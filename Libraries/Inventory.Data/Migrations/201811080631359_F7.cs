namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "Noted", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "Noted");
        }
    }
}
