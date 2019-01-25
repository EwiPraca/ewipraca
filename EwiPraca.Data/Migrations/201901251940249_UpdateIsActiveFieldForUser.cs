namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIsActiveFieldForUser : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Users SET IsActive = 1");
        }
        
        public override void Down()
        {
        }
    }
}
