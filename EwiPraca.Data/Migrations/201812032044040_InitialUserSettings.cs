namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialUserSettings : DbMigration
    {
        public override void Up()
        {
            Sql(DbResources.InitialSettings);
        }
        
        public override void Down()
        {
        }
    }
}
