namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeavesChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SickLeave", newName: "Leave");
            AddColumn("dbo.Leave", "LeaveType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leave", "LeaveType");
            RenameTable(name: "dbo.Leave", newName: "SickLeave");
        }
    }
}
