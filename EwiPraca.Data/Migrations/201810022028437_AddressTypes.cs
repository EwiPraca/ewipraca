namespace EwiPraca.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressTypes : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("INSERT INTO [dbo].[AddressType] ([AddressTypeName], [AddressTypeNameDescription]) VALUES ('{0}','{1}')", Enumerations.AddressType.Zameldowania.ToString(), "Adres zameldowania"));

            Sql(string.Format("INSERT INTO [dbo].[AddressType] ([AddressTypeName], [AddressTypeNameDescription]) VALUES ('{0}','{1}')", Enumerations.AddressType.Korespondencyjny.ToString(), "Adres korespondencyjny"));
        }
        
        public override void Down()
        {
        }
    }
}
