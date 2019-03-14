namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebtNotes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        DebtType = c.Int(nullable: false),
                        Total = c.Double(nullable: false),
                        PaymentType_Id = c.Guid(),
                        PayWarehouse_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentType_Id)
                .ForeignKey("dbo.Warehouses", t => t.PayWarehouse_Id)
                .Index(t => t.PaymentType_Id)
                .Index(t => t.PayWarehouse_Id);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DebtNotes", "PayWarehouse_Id", "dbo.Warehouses");
            DropForeignKey("dbo.DebtNotes", "PaymentType_Id", "dbo.PaymentTypes");
            DropIndex("dbo.DebtNotes", new[] { "PayWarehouse_Id" });
            DropIndex("dbo.DebtNotes", new[] { "PaymentType_Id" });
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.DebtNotes");
        }
    }
}
