namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EwiPracaFilesPartI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EwiFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 260),
                        ContentType = c.String(nullable: false, maxLength: 100),
                        FileType = c.Int(nullable: false),
                        ParentObjectId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ParentObjectId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.EwiFile", new[] { "ParentObjectId" });
            DropTable("dbo.EwiFile");
        }
    }
}
