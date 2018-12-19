namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDocumentsFiles4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedFileLink",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileGuid = c.String(),
                        GuidLink = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserFile", "FileGuid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserFile", new[] { "FileGuid" });
            DropTable("dbo.SharedFileLink");
        }
    }
}
