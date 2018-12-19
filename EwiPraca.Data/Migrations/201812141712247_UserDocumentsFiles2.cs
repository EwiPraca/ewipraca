namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDocumentsFiles2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFile", "FileGuid", c => c.String(nullable: false, maxLength: 36));
            DropColumn("dbo.EwiFile", "FileGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EwiFile", "FileGuid", c => c.String(nullable: false, maxLength: 36));
            DropColumn("dbo.UserFile", "FileGuid");
        }
    }
}
