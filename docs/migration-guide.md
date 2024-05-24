# Database Migration Guide

This guide provides instructions on creating and updating database migrations. It is structured into two primary phases: pre-production (First Development Phase) and post-production (After First Release of Production).

## First Development Phase (Pre-Production)

### Creating and Applying Migrations
During the first development phase, you can directly create migrations in the `ProjectName.Infrastructure.EF` project rather than the `ProjectName.Migrations.SqlServer` project.
This is because the first migration file must be created where the `DbContext` is located.
Then you can move the migrations to the `ProjectName.Migrations.SqlServer` project after the first release and continue to create new migrations there rather than in the `ProjectName.Infrastructure.EF` project.

1. **Create a Migration**:
    - Navigate to the infrastructure `ProjectName.Infrastructure.EF` project.
    - Use the [add-migration.cmd](../src/Alikai.Factoring.Infrastructure.EF/Scripts/add-migration.cmd) script to generate a new migration.
    - Follow the naming convention: `Version_{VersionNumber}`, where `VersionNumber` is a sequential number padded with four zeros (e.g., `Version_0002`).

2. **Apply Migrations to Database**:
    - During this phase, you may delete all old migrations and replace them with a fresh `Version_0001` migration.

3. **Configuration Adjustment**:
    - In the [DbContextHelpers.cs](../src/Alikai.Factoring.Infrastructure.EF/Helpers/DbContextHelpers.cs) file, comment out the following line to prevent assembly mismatch issues:
      ```csharp
      //o.MigrationsAssembly("ProjectName.Migrations.SqlServer"); // Comment this line during the first development phase.
      ```
    - This modification ensures that migrations are correctly associated with the `ProjectName.Infrastructure.EF` project.

### Configuration Code Sample
```csharp
public static void ConfigureSqlServer(string connectionString, DbContextOptionsBuilder options)
{
    options.UseSqlServer(connectionString, o =>
    {
        //Ensure the next line is commented out during the first development phase.
        //o.MigrationsAssembly("ProjectName.Migrations.SqlServer");
        o.EnableRetryOnFailure(8, TimeSpan.FromSeconds(15), null);
    });
}
```

## After First Release of Production (Post-Production)

### Migration Management

1. **Transfer Migrations**:
    - Move all existing migrations from `ProjectName.Infrastructure.EF` to `Alikai.Factoring.Migrations.SqlServer`.

2. **Update Configuration**:
    - Uncomment the previously commented line in the `DbContextHelpers.cs` file to reflect the migration's new location:
      ```csharp
      o.MigrationsAssembly("ProjectName.Migrations.SqlServer"); // Uncomment this line after the first release.
      ```

### Configuration Code Sample
```csharp
public static void ConfigureSqlServer(string connectionString, DbContextOptionsBuilder options)
{
    options.UseSqlServer(connectionString, o =>
    {
        o.MigrationsAssembly("ProjectName.Migrations.SqlServer"); // Ensure this line is uncommented after moving migrations.
        o.EnableRetryOnFailure(8, TimeSpan.FromSeconds(15), null);
    });
}
```

3. **Creating New Migrations**:
    - Post-release, continue to create new migrations using the [add-migration.cmd](../src/Alikai.Factoring.DbMigrator/Scripts/add-migration.cmd) script located in the `ProjectName.DbMigrator` project.
    - Maintain the naming convention `Version_{VersionNumber}` with the appropriate version number and padding.

> [!CAUTION]
> After the first release of production, you should not delete any previous migrations.

## Applying Migrations
- Run the [apply-migration](../src/Alikai.Factoring.DbMigrator/Scripts/apply-migration.cmd) script to apply the migration to the database, or you can simply run the `ProjectName.DbMigratior` manually to apply the migration.

By following these guidelines, you can ensure a consistent and efficient management of database migrations throughout the lifecycle of the project.
