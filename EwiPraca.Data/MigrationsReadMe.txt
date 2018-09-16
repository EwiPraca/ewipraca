Add-Migration InitialMigration -ConfigurationTypeName  EwiPraca.Data.Migrations.Configuration  -Force

Update-Database -ConfigurationTypeName EwiPraca.Data.Migrations.Configuration