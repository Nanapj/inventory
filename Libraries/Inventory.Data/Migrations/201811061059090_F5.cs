namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WarehouseId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WarehouseId");
        }
    }
}
