namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDocumentsFiles3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserFile", "ContentType", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserFile", "ContentType", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
