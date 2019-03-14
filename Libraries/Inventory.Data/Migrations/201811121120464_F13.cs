namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F13 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notes", new[] { "Destination_Id" });
            DropIndex("dbo.Notes", new[] { "Source_Id" });
            AlterColumn("dbo.Notes", "Destination_Id", c => c.Guid());
            AlterColumn("dbo.Notes", "Source_Id", c => c.Guid());
            CreateIndex("dbo.Notes", "Destination_Id");
            CreateIndex("dbo.Notes", "Source_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Notes", new[] { "Source_Id" });
            DropIndex("dbo.Notes", new[] { "Destination_Id" });
            AlterColumn("dbo.Notes", "Source_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Notes", "Destination_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.Notes", "Source_Id");
            CreateIndex("dbo.Notes", "Destination_Id");
        }
    }
}
