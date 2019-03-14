namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UnitName = c.String(),
                        Delete = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        Index = c.Long(),
                        MonthIndex = c.Long(),
                        Product_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Warehouses", "WhCode", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Units", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Units", "Product_Id", "dbo.Products");
            DropIndex("dbo.Units", new[] { "User_Id" });
            DropIndex("dbo.Units", new[] { "Product_Id" });
            DropColumn("dbo.Warehouses", "WhCode");
            DropTable("dbo.Units");
        }
    }
}
