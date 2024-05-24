using Company.Project.Domain.Entities;
using Company.Project.Infrastructure.Data.ModelConfigurations;
using Company.Project.Infrastructure.Interceptors;
using Company.Project.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<
    User, 
    Role, 
    string, 
    IdentityUserClaim<string>, 
    UserRole, 
    IdentityUserLogin<string>, 
    RoleClaim, 
    IdentityUserToken<string>>
{
    private readonly ApplicationDbContextOptions _dbContextOptions;
    private readonly AuditingEntitiesInterceptor? _auditingEntitiesInterceptor;
    private readonly DispatchDomainEventsInterceptor? _dispatchDomainEventsInterceptor;
    private readonly IServiceProvider? _serviceProvider;

    public ApplicationDbContext(
        ApplicationDbContextOptions dbContextOptions,
        AuditingEntitiesInterceptor? auditingEntitiesInterceptor = default,
        DispatchDomainEventsInterceptor? dispatchDomainEventsInterceptor = default,
        IServiceProvider? serviceProvider = default)
    {
        _auditingEntitiesInterceptor = auditingEntitiesInterceptor;
        _dispatchDomainEventsInterceptor = dispatchDomainEventsInterceptor;
        _dbContextOptions = dbContextOptions;
        _serviceProvider = serviceProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var availableDatabaseProviders = new List<string>();
        
        if (_dispatchDomainEventsInterceptor is not null)
        {
            options.AddInterceptors(_dispatchDomainEventsInterceptor);
        }
        if (_auditingEntitiesInterceptor is not null)
        {
            options.AddInterceptors(_auditingEntitiesInterceptor);
        }

        if (string.IsNullOrEmpty(_dbContextOptions.DatabaseProvider))
        {
            throw new ArgumentException("The database provider is not specified");
        }
        if (string.IsNullOrEmpty(_dbContextOptions.ConnectionString))
        {
            throw new ArgumentException("The connection string is not specified");
        }

#if SqlServer
        availableDatabaseProviders.Add("SqlServer");

        if (_dbContextOptions.DatabaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            DbContextHelpers.ConfigureSqlServer(_dbContextOptions.ConnectionString, options);
        }
#endif
        
#if Sqlite
        availableDatabaseProviders.Add("Sqlite");        

        if (_dbContextOptions.DatabaseProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
        {
            DbContextHelpers.ConfigureSqlite(_dbContextOptions.ConnectionString, options);
        }
#endif
        
#if PostgreSql
        availableDatabaseProviders.Add("PostgreSql");

        if (_dbContextOptions.DatabaseProvider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase))
        {
            DbContextHelpers.ConfigurePostgreSQL(_dbContextOptions.ConnectionString, options);
        }
#endif 
        
#if MySql
        availableDatabaseProviders.Add("MySql");

        if (_dbContextOptions.DatabaseProvider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
        {
            DbContextHelpers.ConfigureMySql(_dbContextOptions.ConnectionString, options);
        }
#endif
        
        if (!_dbContextOptions.DatabaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase) &&
            !_dbContextOptions.DatabaseProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase) &&
            !_dbContextOptions.DatabaseProvider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase) &&
            !_dbContextOptions.DatabaseProvider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The database provider is invalid. The available database providers are: " +
                                    string.Join(", ", availableDatabaseProviders));
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new RoleClaimConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());
    }

    public TService? GetService<TService>()
    {
        if (_serviceProvider is null)
        {
            return default;
        }
        
        return (TService?)_serviceProvider.GetService(typeof(TService));
    }

    public TService GetRequiredService<TService>()
    {
        var service = GetService<TService>();

        if (service is null)
        {
            throw new InvalidOperationException($"The required service {typeof(TService).Name} is not registered");
        }
        
        return service;
    }
}
