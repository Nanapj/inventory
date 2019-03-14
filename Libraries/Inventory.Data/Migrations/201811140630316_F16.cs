namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Warehouses", "WarehouseCode", c => c.String());
            DropColumn("dbo.Warehouses", "WhCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Warehouses", "WhCode", c => c.String());
            DropColumn("dbo.Warehouses", "WarehouseCode");
        }
    }
}
