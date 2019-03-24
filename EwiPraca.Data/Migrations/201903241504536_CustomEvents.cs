namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        Title = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCompany", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomEvent", "CompanyId", "dbo.UserCompany");
            DropIndex("dbo.CustomEvent", new[] { "CompanyId" });
            DropTable("dbo.CustomEvent");
        }
    }
}
