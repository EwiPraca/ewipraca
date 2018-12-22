namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetPasswordFunctionality : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetPasswordRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(maxLength: 128),
                        ResetGuid = c.String(nullable: false, maxLength: 128),
                        MailSentDate = c.DateTime(),
                        IsCompleted = c.Boolean(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResetPasswordRequest", "ApplicationUserID", "dbo.Users");
            DropIndex("dbo.ResetPasswordRequest", new[] { "ApplicationUserID" });
            DropTable("dbo.ResetPasswordRequest");
        }
    }
}
