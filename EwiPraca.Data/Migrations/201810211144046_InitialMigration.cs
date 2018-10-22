namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressTypeName = c.String(nullable: false, maxLength: 30),
                        AddressTypeNameDescription = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false, maxLength: 100),
                        StreetName = c.String(nullable: false, maxLength: 100),
                        StreetNumber = c.String(nullable: false, maxLength: 10),
                        PlaceNumber = c.String(maxLength: 10),
                        ZIPCode = c.String(nullable: false, maxLength: 10),
                        AddressTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressType", t => t.AddressTypeId)
                .Index(t => t.AddressTypeId);
            
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        ContractType = c.Int(nullable: false),
                        Workplace = c.String(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JobPart = c.String(),
                        Notes = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        Surname = c.String(nullable: false, maxLength: 100),
                        PESEL = c.String(nullable: false, maxLength: 200),
                        BirthDate = c.DateTime(nullable: false),
                        AddressId = c.Int(nullable: false),
                        UserCompanyId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.UserCompany", t => t.UserCompanyId)
                .Index(t => t.AddressId)
                .Index(t => t.UserCompanyId);
            
            CreateTable(
                "dbo.UserCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 100),
                        REGON = c.String(nullable: false, maxLength: 20),
                        KRS = c.String(maxLength: 20),
                        NIP = c.String(nullable: false, maxLength: 20),
                        Notes = c.String(maxLength: 1000),
                        ApplicationUserID = c.String(maxLength: 128),
                        AddressId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.Users", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            Sql(string.Format("INSERT INTO [dbo].[AddressType] ([AddressTypeName], [AddressTypeNameDescription]) VALUES ('{0}','{1}')", Enumerations.AddressType.Zameldowania.ToString(), "Adres zameldowania"));

            Sql(string.Format("INSERT INTO [dbo].[AddressType] ([AddressTypeName], [AddressTypeNameDescription]) VALUES ('{0}','{1}')", Enumerations.AddressType.Korespondencyjny.ToString(), "Adres korespondencyjny"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Contract", "EmployeeId", "dbo.Employee");
            DropForeignKey("dbo.Employee", "UserCompanyId", "dbo.UserCompany");
            DropForeignKey("dbo.UserCompany", "ApplicationUserID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCompany", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Employee", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Address", "AddressTypeId", "dbo.AddressType");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.UserCompany", new[] { "AddressId" });
            DropIndex("dbo.UserCompany", new[] { "ApplicationUserID" });
            DropIndex("dbo.Employee", new[] { "UserCompanyId" });
            DropIndex("dbo.Employee", new[] { "AddressId" });
            DropIndex("dbo.Contract", new[] { "EmployeeId" });
            DropIndex("dbo.Address", new[] { "AddressTypeId" });
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.UserCompany");
            DropTable("dbo.Employee");
            DropTable("dbo.Contract");
            DropTable("dbo.Address");
            DropTable("dbo.AddressType");
        }
    }
}
