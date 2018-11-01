namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeePosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "PositionDictionaryValueId", c => c.Int());
            CreateIndex("dbo.Employee", "PositionDictionaryValueId");
            AddForeignKey("dbo.Employee", "PositionDictionaryValueId", "dbo.PositionDictionaryValue", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "PositionDictionaryValueId", "dbo.PositionDictionaryValue");
            DropIndex("dbo.Employee", new[] { "PositionDictionaryValueId" });
            DropColumn("dbo.Employee", "PositionDictionaryValueId");
        }
    }
}
