namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarOccurency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomEvent", "IsOccurency", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomEvent", "IsOccurency");
        }
    }
}
