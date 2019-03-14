namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notes", "Index", c => c.Long());
            AlterColumn("dbo.Warehouses", "Index", c => c.Long());
            AlterColumn("dbo.NoteDetails", "Index", c => c.Long());
            AlterColumn("dbo.Products", "Index", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Index", c => c.Int());
            AlterColumn("dbo.NoteDetails", "Index", c => c.Int());
            AlterColumn("dbo.Warehouses", "Index", c => c.Int());
            AlterColumn("dbo.Notes", "Index", c => c.Int());
        }
    }
}
