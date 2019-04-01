namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShareCalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCompany", "CalendarGuid", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCompany", "CalendarGuid");
        }
    }
}
