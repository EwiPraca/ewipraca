Add-Migration InitialMigration -ConfigurationTypeName  EwiPraca.Data.Migrations.Configuration  -Force

Update-Database -ConfigurationTypeName EwiPraca.Data.Migrations.Configuration

Update-Database -TargetMigration 201811092209026_EmployeeSex -ConfigurationTypeName EwiPraca.Data.Migrations.Configuration