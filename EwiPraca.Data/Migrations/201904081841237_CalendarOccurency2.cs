namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarOccurency2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomEvent", "OccurencyIntervalNumber", c => c.Int());
            AddColumn("dbo.CustomEvent", "OccurenceType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomEvent", "OccurenceType");
            DropColumn("dbo.CustomEvent", "OccurencyIntervalNumber");
        }
    }
}
