@echo off

cd ../
echo Do you want to remove the latest migration (y/n):
set /p UserPromptResult=

if /I "%UserPromptResult%" == "y" (

	rem #if SqlServer
    echo Removing the latest migration from SQL Server...
    dotnet ef migrations remove --project ../Company.Project.Migrations.SqlServer -- --provider SqlServer
    rem #endif
    
    rem #if PostgreSql
    echo Removing the latest migration from PostgreSQL...
    dotnet ef migrations remove --project ../Company.Project.Migrations.PostgreSql -- --provider PostgreSql
    rem #endif
    
    rem #if MySql
    echo Removing the latest migration from MySQL...
    dotnet ef migrations remove --project ../Company.Project.Migrations.MySql -- --provider MySql
    rem #endif
    
    rem #if Sqlite
    echo Removing the latest migration from Sqlite...
    dotnet ef migrations remove --project ../Company.Project.Migrations.Sqlite -- --provider Sqlite
    rem #endif
)

echo Removed the latest migration
pause
