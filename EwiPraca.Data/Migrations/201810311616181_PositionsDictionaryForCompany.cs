namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionsDictionaryForCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PositionDictionary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserCompanyId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCompany", t => t.UserCompanyId, cascadeDelete: true)
                .Index(t => t.UserCompanyId, unique: true);
            
            CreateTable(
                "dbo.PositionDictionaryValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionDictionaryId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PositionDictionary", t => t.PositionDictionaryId, cascadeDelete: true)
                .Index(t => t.PositionDictionaryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PositionDictionaryValue", "PositionDictionaryId", "dbo.PositionDictionary");
            DropForeignKey("dbo.PositionDictionary", "UserCompanyId", "dbo.UserCompany");
            DropIndex("dbo.PositionDictionaryValue", new[] { "PositionDictionaryId" });
            DropIndex("dbo.PositionDictionary", new[] { "UserCompanyId" });
            DropTable("dbo.PositionDictionaryValue");
            DropTable("dbo.PositionDictionary");
        }
    }
}
