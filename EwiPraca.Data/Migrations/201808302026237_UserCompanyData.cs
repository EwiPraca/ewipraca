namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCompanyData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 100),
                        REGON = c.String(nullable: false, maxLength: 20),
                        KRS = c.String(maxLength: 20),
                        NIP = c.String(nullable: false, maxLength: 20),
                        ApplicationUserID = c.String(maxLength: 128),
                        UserCompanyAdressId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUserID)
                .ForeignKey("dbo.UserCompanyAddress", t => t.UserCompanyAdressId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.UserCompanyAdressId);
            
            CreateTable(
                "dbo.UserCompanyAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false, maxLength: 100),
                        StreetName = c.String(nullable: false, maxLength: 100),
                        StreetNumber = c.String(nullable: false, maxLength: 10),
                        PlaceNumber = c.String(maxLength: 10),
                        ZIPCode = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCompany", "UserCompanyAdressId", "dbo.UserCompanyAddress");
            DropForeignKey("dbo.UserCompany", "ApplicationUserID", "dbo.Users");
            DropIndex("dbo.UserCompany", new[] { "UserCompanyAdressId" });
            DropIndex("dbo.UserCompany", new[] { "ApplicationUserID" });
            DropTable("dbo.UserCompanyAddress");
            DropTable("dbo.UserCompany");
        }
    }
}
