namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDocumentsFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 260),
                        ContentType = c.String(nullable: false, maxLength: 100),
                        ParentFileId = c.Int(),
                        ApplicationUserID = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUserID)
                .ForeignKey("dbo.UserFile", t => t.ParentFileId)
                .Index(t => t.ParentFileId)
                .Index(t => t.ApplicationUserID);
            
            AddColumn("dbo.EwiFile", "FileGuid", c => c.String(nullable: false, maxLength: 36));
            AddColumn("dbo.EwiFile", "ApplicationUserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.EwiFile", "ContentType", c => c.String(maxLength: 100));
            CreateIndex("dbo.EwiFile", "ApplicationUserID");
            AddForeignKey("dbo.EwiFile", "ApplicationUserID", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFile", "ParentFileId", "dbo.UserFile");
            DropForeignKey("dbo.UserFile", "ApplicationUserID", "dbo.Users");
            DropForeignKey("dbo.EwiFile", "ApplicationUserID", "dbo.Users");
            DropIndex("dbo.UserFile", new[] { "ApplicationUserID" });
            DropIndex("dbo.UserFile", new[] { "ParentFileId" });
            DropIndex("dbo.EwiFile", new[] { "ApplicationUserID" });
            AlterColumn("dbo.EwiFile", "ContentType", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.EwiFile", "ApplicationUserID");
            DropColumn("dbo.EwiFile", "FileGuid");
            DropTable("dbo.UserFile");
        }
    }
}
