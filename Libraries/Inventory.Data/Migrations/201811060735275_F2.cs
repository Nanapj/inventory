namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        Total = c.Double(nullable: false),
                        Delete = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        Index = c.Int(),
                        MonthIndex = c.Long(),
                        Destination_Id = c.Guid(nullable: false),
                        Source_Id = c.Guid(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.Destination_Id)
                .ForeignKey("dbo.Warehouses", t => t.Source_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Destination_Id)
                .Index(t => t.Source_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.NoteDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Delete = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        Index = c.Int(),
                        MonthIndex = c.Long(),
                        Product_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                        Note_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Notes", t => t.Note_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Note_Id);
            
            AddColumn("dbo.Products", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notes", "Source_Id", "dbo.Warehouses");
            DropForeignKey("dbo.NoteDetails", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.NoteDetails", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NoteDetails", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Notes", "Destination_Id", "dbo.Warehouses");
            DropIndex("dbo.NoteDetails", new[] { "Note_Id" });
            DropIndex("dbo.NoteDetails", new[] { "User_Id" });
            DropIndex("dbo.NoteDetails", new[] { "Product_Id" });
            DropIndex("dbo.Notes", new[] { "User_Id" });
            DropIndex("dbo.Notes", new[] { "Source_Id" });
            DropIndex("dbo.Notes", new[] { "Destination_Id" });
            DropColumn("dbo.Products", "Code");
            DropTable("dbo.NoteDetails");
            DropTable("dbo.Notes");
        }
    }
}
