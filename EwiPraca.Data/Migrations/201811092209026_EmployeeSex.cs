namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeSex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "Sex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "Sex");
        }
    }
}
