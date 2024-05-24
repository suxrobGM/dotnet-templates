#!/bin/bash

cd ../
echo "Enter Migration Name: "
read -r MigrationName

if [ -z "$MigrationName" ]
then
    echo "Error: Migration name cannot be empty."
    exit 1
fi

//#if (SqlServer)
echo "Running migration for SQL Server..."
dotnet ef migrations add "$MigrationName" --project ../Company.Project.Migrations.SqlServer -- --provider SqlServer
//#endif

//#if (PostgreSql)
echo "Running migration for PostgreSQL..."
dotnet ef migrations add "$MigrationName" --project ../Company.Project.Migrations.PostgreSql -- --provider PostgreSql
//#endif

//#if (MySql)
echo "Running migration for MySQL..."
dotnet ef migrations add "$MigrationName" --project ../Company.Project.Migrations.MySql -- --provider MySql
//#endif

//#if (Sqlite)
echo "Running migration for Sqlite..."
dotnet ef migrations add "$MigrationName" --project ../Company.Project.Migrations.Sqlite -- --provider Sqlite
//#endif

echo "Migrations completed."

echo "Do you want to apply migrations (y/n): "
read -r ApplyMigrationResult

if [ "$ApplyMigrationResult" = "y" ]
then
    cd ./Scripts || exit
    ./apply-migration.sh
fi
