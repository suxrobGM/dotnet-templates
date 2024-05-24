#!/bin/bash

cd ../
echo "Do you want to remove the latest migration (y/n): "
read -r UserPromptResult

if [ "$UserPromptResult" = "y" ]
then
//#if (SqlServer)
    echo "Removing the latest migration from SQL Server..."
    dotnet ef migrations remove --project ../Company.Project.Migrations.SqlServer -- --provider SqlServer
//#endif

//#if (PostgreSql)
    echo "Removing the latest migration from PostgreSQL..."
    dotnet ef migrations remove --project ../Company.Project.Migrations.PostgreSql -- --provider PostgreSql
//#endif

//#if (MySql)
    echo "Removing the latest migration from MySQL..."
    dotnet ef migrations remove --project ../Company.Project.Migrations.MySql -- --provider MySql
//#endif

//#if (Sqlite)
    echo "Removing the latest migration from Sqlite..."
    dotnet ef migrations remove --project ../Company.Project.Migrations.Sqlite -- --provider Sqlite
//#endif
fi

echo "Removed the latest migration"
