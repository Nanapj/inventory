namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F12 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notes", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notes", "Type", c => c.Int(nullable: false));
        }
    }
}
