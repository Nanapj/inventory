namespace Inventory.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class F10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DebtNotes", "Delete", c => c.Boolean(nullable: false));
            AddColumn("dbo.DebtNotes", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DebtNotes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DebtNotes", "Index", c => c.Long());
            AddColumn("dbo.DebtNotes", "MonthIndex", c => c.Long());
            AddColumn("dbo.DebtNotes", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PaymentTypes", "Delete", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTypes", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentTypes", "LastModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentTypes", "Index", c => c.Long());
            AddColumn("dbo.PaymentTypes", "MonthIndex", c => c.Long());
            AddColumn("dbo.PaymentTypes", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DebtNotes", "User_Id");
            CreateIndex("dbo.PaymentTypes", "User_Id");
            AddForeignKey("dbo.PaymentTypes", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DebtNotes", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DebtNotes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentTypes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PaymentTypes", new[] { "User_Id" });
            DropIndex("dbo.DebtNotes", new[] { "User_Id" });
            DropColumn("dbo.PaymentTypes", "User_Id");
            DropColumn("dbo.PaymentTypes", "MonthIndex");
            DropColumn("dbo.PaymentTypes", "Index");
            DropColumn("dbo.PaymentTypes", "LastModifiedDate");
            DropColumn("dbo.PaymentTypes", "CreatedDate");
            DropColumn("dbo.PaymentTypes", "Delete");
            DropColumn("dbo.DebtNotes", "User_Id");
            DropColumn("dbo.DebtNotes", "MonthIndex");
            DropColumn("dbo.DebtNotes", "Index");
            DropColumn("dbo.DebtNotes", "LastModifiedDate");
            DropColumn("dbo.DebtNotes", "CreatedDate");
            DropColumn("dbo.DebtNotes", "Delete");
        }
    }
}
