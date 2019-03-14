namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "Type");
        }
    }
}
