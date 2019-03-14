namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DebtNotes", "IsOwed", c => c.Boolean(nullable: false));
            DropColumn("dbo.DebtNotes", "DebtType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DebtNotes", "DebtType", c => c.Int(nullable: false));
            DropColumn("dbo.DebtNotes", "IsOwed");
        }
    }
}
