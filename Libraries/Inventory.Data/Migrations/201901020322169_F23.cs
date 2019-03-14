namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "ProductType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "ProductType");
        }
    }
}
