namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeMedicalReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalReport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        CompletionDate = c.DateTime(nullable: false),
                        NextCompletionDate = c.DateTime(),
                        Result = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalReport", "EmployeeId", "dbo.Employee");
            DropIndex("dbo.MedicalReport", new[] { "EmployeeId" });
            DropTable("dbo.MedicalReport");
        }
    }
}
