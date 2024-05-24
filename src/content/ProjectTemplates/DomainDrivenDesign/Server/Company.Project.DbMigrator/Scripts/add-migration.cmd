@echo off

cd ../

:prompt
set "MigrationName="
set /p MigrationName="Enter Migration Name: "

if "%MigrationName%" == "" (
    echo Error: Migration name cannot be empty.
    goto prompt
)

rem #if SqlServer
echo Running migration for SQL Server...
dotnet ef migrations add %MigrationName% --project ../Company.Project.Migrations.SqlServer -- --provider SqlServer
rem #endif

rem #if PostgreSql
echo Running migration for PostgreSQL...
dotnet ef migrations add %MigrationName% --project ../Company.Project.Migrations.PostgreSql -- --provider PostgreSql
rem #endif

rem #if MySql
echo Running migration for MySQL...
dotnet ef migrations add %MigrationName% --project ../Company.Project.Migrations.MySql -- --provider MySql
rem #endif

rem #if Sqlite
echo Running migration for Sqlite...
dotnet ef migrations add %MigrationName% --project ../Company.Project.Migrations.Sqlite -- --provider Sqlite
rem #endif

echo Migrations completed.

echo Do you want to apply migrations (y/n):
set /p ApplyMigrationResult=

if /I "%ApplyMigrationResult%" == "y" (
    cd ./Scripts
	call ./apply-migration.cmd
)

pause
