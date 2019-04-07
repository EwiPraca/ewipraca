namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarReminder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomEvent", "Reminder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomEvent", "Reminder");
        }
    }
}
