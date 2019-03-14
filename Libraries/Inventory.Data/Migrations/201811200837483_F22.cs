namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DebtNotes", "Noted", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DebtNotes", "Noted");
        }
    }
}
