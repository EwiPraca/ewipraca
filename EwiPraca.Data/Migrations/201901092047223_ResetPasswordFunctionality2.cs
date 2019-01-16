namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetPasswordFunctionality2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmailMessage", "EmployeeId", "dbo.Employee");
            DropIndex("dbo.EmailMessage", new[] { "EmployeeId" });
            AlterColumn("dbo.EmailMessage", "EmployeeId", c => c.Int());
            CreateIndex("dbo.EmailMessage", "EmployeeId");
            AddForeignKey("dbo.EmailMessage", "EmployeeId", "dbo.Employee", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailMessage", "EmployeeId", "dbo.Employee");
            DropIndex("dbo.EmailMessage", new[] { "EmployeeId" });
            AlterColumn("dbo.EmailMessage", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.EmailMessage", "EmployeeId");
            AddForeignKey("dbo.EmailMessage", "EmployeeId", "dbo.Employee", "Id", cascadeDelete: true);
        }
    }
}
