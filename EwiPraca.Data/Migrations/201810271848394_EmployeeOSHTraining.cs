namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeOSHTraining : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OSHTraining",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        CompletionDate = c.DateTime(nullable: false),
                        NextCompletionDate = c.DateTime(),
                        Notes = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            AddColumn("dbo.MedicalReport", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OSHTraining", "EmployeeId", "dbo.Employee");
            DropIndex("dbo.OSHTraining", new[] { "EmployeeId" });
            DropColumn("dbo.MedicalReport", "Notes");
            DropTable("dbo.OSHTraining");
        }
    }
}
