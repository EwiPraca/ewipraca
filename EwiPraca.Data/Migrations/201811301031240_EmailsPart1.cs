namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailsPart1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailMessage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                        Recipient = c.String(),
                        SentDate = c.DateTime(),
                        EmailType = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUserID)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId)
                .Index(t => t.ApplicationUserID);
            
            AddColumn("dbo.MedicalReport", "ReminderSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.OSHTraining", "ReminderSent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailMessage", "EmployeeId", "dbo.Employee");
            DropForeignKey("dbo.EmailMessage", "ApplicationUserID", "dbo.Users");
            DropIndex("dbo.EmailMessage", new[] { "ApplicationUserID" });
            DropIndex("dbo.EmailMessage", new[] { "EmployeeId" });
            DropColumn("dbo.OSHTraining", "ReminderSent");
            DropColumn("dbo.MedicalReport", "ReminderSent");
            DropTable("dbo.EmailMessage");
        }
    }
}
