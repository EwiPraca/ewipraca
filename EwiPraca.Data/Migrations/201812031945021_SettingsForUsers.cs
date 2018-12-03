namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingsForUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSetting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        SettingId = c.Int(nullable: false),
                        SettingValue = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUserID, cascadeDelete: true)
                .ForeignKey("dbo.Setting", t => t.SettingId, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.SettingId);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingName = c.String(nullable: false, maxLength: 100),
                        SettingDescription = c.String(nullable: false, maxLength: 200),
                        SettingValueType = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSetting", "SettingId", "dbo.Setting");
            DropForeignKey("dbo.UserSetting", "ApplicationUserID", "dbo.Users");
            DropIndex("dbo.UserSetting", new[] { "SettingId" });
            DropIndex("dbo.UserSetting", new[] { "ApplicationUserID" });
            DropTable("dbo.Setting");
            DropTable("dbo.UserSetting");
        }
    }
}
