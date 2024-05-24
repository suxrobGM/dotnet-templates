using Microsoft.EntityFrameworkCore.Design;

namespace Company.Project.Infrastructure.Data;

/// <summary>
/// ApplicationDbContextFactory is a factory class for creating instances of ApplicationDbContext
/// with dynamically determined database providers at design time. This is primarily used for 
/// generating database migrations specific to a given database provider.
/// 
/// The database provider is specified via command-line arguments when running EF Core migration commands.
/// 
/// CLI Usage:
/// To create migrations for a specific database provider, pass the <c>'--provider'</c> argument
/// with the provider name. Supported providers are <c>'SqlServer'</c>, <c>'PostgreSql'</c>, <c>'MySql'</c>, <c>'Sqlite'</c>.
/// 
/// Example for SQL Server:
/// <code>dotnet ef migrations add Version_0001 --project ../Company.Project.Migrations.SqlServer -- --provider SqlServer</code>
/// 
/// Example for SQLite:
/// <code>dotnet ef migrations add Version_0001 --project ../Company.Project.Migrations.Sqlite -- --provider Sqlite</code>
///
/// /// Example for PostgreSQL:
/// <code>dotnet ef migrations add Version_0001 --project ../Company.Project.Migrations.PostgreSql -- --provider PostgreSql</code>
///
/// /// Example for MySQL:
/// <code>dotnet ef migrations add Version_0001 --project ../Company.Project.Migrations.MySql -- --provider MySql</code>
/// 
/// The class parses the command-line arguments to determine the provider and sets up
/// the corresponding database connection string and provider-specific options.
/// </summary>
internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new instance of ApplicationDbContext with the appropriate configuration
    /// based on the specified database provider.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the factory, used to determine the database provider.</param>
    /// <returns>A configured instance of ApplicationDbContext.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid or unspecified database provider is passed.</exception>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var provider = ParseProvider(args);

        var contextOptions = provider switch
        {
            "Sqlite" => new ApplicationDbContextOptions
            {
                DatabaseProvider = "Sqlite",
                ConnectionString = "{CONNECTION_STRING}" //"Data Source=Data\\MyDatabase.sqlite"
            },
            "SqlServer" => new ApplicationDbContextOptions
            {
                DatabaseProvider = "SqlServer",
                ConnectionString = "{CONNECTION_STRING}" //"Data Source=.\\SQLEXPRESS; Database=MyDatabase; Integrated Security=true; TrustServerCertificate=true"
            },
            "PostgreSql" => new ApplicationDbContextOptions
            {
                DatabaseProvider = "PostgreSql",
                ConnectionString = "{CONNECTION_STRING}" //"Data Source=localhost; Port=5432; Database=MyDatabase; Username=postgres; Password=postgres"
            },
            "MySql" => new ApplicationDbContextOptions
            {
                DatabaseProvider = "MySql",
                ConnectionString = "{CONNECTION_STRING}" //"Data Source=localhost; Port=3306; Database=MyDatabase; Username=root; Password=root"
            },
            _ => throw new ArgumentException("Invalid or unspecified database provider. Please specify --provider with 'SqlServer', 'Sqlite', 'PostgreSql' or 'MySql'")
        };

        return new ApplicationDbContext(contextOptions);
    }
    
    private static string ParseProvider(string[] args)
    {
        var providerArgIndex = Array.FindIndex(args, arg => arg.Equals("--provider", StringComparison.OrdinalIgnoreCase));
        
        if (providerArgIndex >= 0 && providerArgIndex < args.Length - 1)
        {
            return args[providerArgIndex + 1].Trim();
        }

        throw new ArgumentException("No database provider specified. Use '--provider' argument followed by the provider name.");
    }
}
