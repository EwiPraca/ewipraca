namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobPartDictionaryValuesForContract : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobPartDictionaryValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobPartDictionaryId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobPartDictionary", t => t.JobPartDictionaryId)
                .Index(t => t.JobPartDictionaryId);
            
            CreateTable(
                "dbo.JobPartDictionary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserCompanyId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCompany", t => t.UserCompanyId)
                .Index(t => t.UserCompanyId, unique: true);
            
            AddColumn("dbo.Contract", "JobPartDictionaryValueId", c => c.Int());
            CreateIndex("dbo.Contract", "JobPartDictionaryValueId");
            AddForeignKey("dbo.Contract", "JobPartDictionaryValueId", "dbo.JobPartDictionaryValue", "Id");
            DropColumn("dbo.Contract", "JobPart");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contract", "JobPart", c => c.String());
            DropForeignKey("dbo.Contract", "JobPartDictionaryValueId", "dbo.JobPartDictionaryValue");
            DropForeignKey("dbo.JobPartDictionaryValue", "JobPartDictionaryId", "dbo.JobPartDictionary");
            DropForeignKey("dbo.JobPartDictionary", "UserCompanyId", "dbo.UserCompany");
            DropIndex("dbo.JobPartDictionary", new[] { "UserCompanyId" });
            DropIndex("dbo.JobPartDictionaryValue", new[] { "JobPartDictionaryId" });
            DropIndex("dbo.Contract", new[] { "JobPartDictionaryValueId" });
            DropColumn("dbo.Contract", "JobPartDictionaryValueId");
            DropTable("dbo.JobPartDictionary");
            DropTable("dbo.JobPartDictionaryValue");
        }
    }
}
