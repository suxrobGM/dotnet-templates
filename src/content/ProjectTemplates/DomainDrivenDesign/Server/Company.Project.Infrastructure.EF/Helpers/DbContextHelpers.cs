using Microsoft.EntityFrameworkCore;

namespace Company.Project.Infrastructure.Helpers;

internal static class DbContextHelpers
{
#if SqlServer
    public static void ConfigureSqlServer(string connectionString, DbContextOptionsBuilder options)
    {
        options.UseSqlServer(connectionString, o =>
        {
            o.MigrationsAssembly("Company.Project.Migrations.SqlServer");
            o.EnableRetryOnFailure(8, TimeSpan.FromSeconds(15), null);
        });
    }
#elif Sqlite
    public static void ConfigureSqlite(string connectionString, DbContextOptionsBuilder options)
    {
        options.UseSqlite(connectionString, o =>
        {
            o.MigrationsAssembly("Company.Project.Migrations.Sqlite");
        });
    }
#elif PostgreSql
    public static void ConfigurePostgreSQL(string connectionString, DbContextOptionsBuilder options)
    {
        options.UseNpgsql(connectionString, o =>
        {
            o.MigrationsAssembly("Company.Project.Migrations.PostgreSql");
        });
    }
#elif MySql
    public static void ConfigureMySql(string connectionString, DbContextOptionsBuilder options)
    {
        options.UseMySql(connectionString, o =>
        {
            o.MigrationsAssembly("Company.Project.Migrations.MySql");
        });
    }
#endif
}
