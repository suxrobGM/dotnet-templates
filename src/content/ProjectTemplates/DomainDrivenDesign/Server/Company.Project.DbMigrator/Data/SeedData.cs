using System.Security.Claims;
using Company.Project.Domain.Entities;
using Company.Project.Infrastructure.Data;
using Company.Project.Shared.Policies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.DbMigrator.Data;

/// <summary>
/// The partial class for seeding the database with initial data.
/// </summary>
public partial class SeedData : BackgroundService
{
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<SeedData> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IServiceProvider _scopeSp = default!;
    
    public SeedData(
        IHostApplicationLifetime appLifetime,
        IConfiguration configuration,
        ILogger<SeedData> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _appLifetime = appLifetime;
        _configuration = configuration;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        await SeedDatabaseAsync(environment ?? "Development");
        _appLifetime.StopApplication();
    }

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <param name="environment">
    /// The environment in which the application is running.
    /// Either 'Development' or 'Production'.
    /// </param>
    private async Task SeedDatabaseAsync(string environment = "Development")
    {
        var scope = _serviceScopeFactory.CreateScope();
        _scopeSp = scope.ServiceProvider;
        var applicationDbContext = _scopeSp.GetRequiredService<ApplicationDbContext>();
        
        _logger.LogInformation("Initializing the database...");
        await MigrateDatabaseAsync(applicationDbContext);
        _logger.LogInformation("Successfully initialized the database");
        
        switch (environment)
        {
            case "Development":
                await SeedDatabaseAsync_Development();
                break;
            case "Production":
                await SeedDatabaseAsync_Production();
                break;
        }
    }
    
    /// <summary>
    /// Migrates the database to the latest version.
    /// </summary>
    /// <param name="databaseContext"></param>
    private static async Task MigrateDatabaseAsync(DbContext databaseContext)
    {
        await databaseContext.Database.MigrateAsync();
    }

    /// <summary>
    /// Adds application roles to the database.
    /// Currently, the application roles are 'Admin', 'Reader', and 'Writer'.
    /// </summary>
    private async Task AddAppRolesAsync()
    {
        using var roleManager = _scopeSp.GetRequiredService<RoleManager<Role>>();
        var appRoles = AppRoles.GetRoleNames();

        foreach (var roleName in appRoles)
        {
            var role = new Role
            {
                Name = roleName
            };

            var existingRole = await roleManager.FindByNameAsync(role.Name);
            
            if (existingRole is not null)
            {
                // Update existing role claims
                await AddPermissionClaimsForRole(roleManager, existingRole);
                _logger.LogInformation("Updated app role '{Role}'", existingRole.Name);
            }
            else
            {
                // Add new role and its claims
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    await AddPermissionClaimsForRole(roleManager, role);
                    _logger.LogInformation("Added the '{RoleName}' role", role.Name);
                }
                else
                {
                    _logger.LogError("Failed to add the '{RoleName}' role", role.Name);
                }
            }
        }
    }

    /// <summary>
    /// Adds the super admin user to the database.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the admin user data is not specified in the appsettings.json file.
    /// </exception>
    private async Task AddSuperAdminAsync()
    {
        var userManager = _scopeSp.GetRequiredService<UserManager<User>>();
        var adminData = _configuration.GetRequiredSection("Admin").Get<UserDto>();

        if (adminData is null)
        {
            throw new InvalidOperationException("Admin user data is null, specify admin user data in the appsettings.json file");
        }
        
        var adminUser = await userManager.FindByEmailAsync(adminData.Email);
        
        if (adminUser is null)
        {
            adminUser = new User
            {
                UserName = adminData.Email,
                Email = adminData.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminData.Password);
            
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }
            
            _logger.LogInformation("Created a super admin user '{Admin}'", adminUser.UserName);
        }

        var hasAdminRole = await userManager.IsInRoleAsync(adminUser, AppRoles.SuperAdmin);
        
        if (!hasAdminRole)
        {
            await userManager.AddToRoleAsync(adminUser, AppRoles.SuperAdmin);
            _logger.LogInformation("Added 'SuperAdmin' role to the user '{Admin}'", adminUser.UserName);
        }
    }
    
    /// <summary>
    /// Adds permission claims to the role.
    /// </summary>
    /// <param name="roleManager">Instance of <see cref="RoleManager{Role}"/> to add permission claims.</param>
    /// <param name="role">The role to which permission claims will be added.</param>
    private async Task AddPermissionClaimsForRole(RoleManager<Role> roleManager, Role role)
    {
        var permissions = AppRolePermissions.GetRolePermissions(role.Name!);
        
        foreach (var permission in permissions)
        {
            await AddPermissionClaimForRole(roleManager, role, permission);
        }
    }
    
    /// <summary>
    /// Adds a permission claim to the role.
    /// </summary>
    /// <param name="roleManager">Instance of <see cref="RoleManager{Role}"/> to add permission claim.</param>
    /// <param name="role">The role to which permission claim will be added.</param>
    /// <param name="permission">The permission to be added as a claim to the role.</param>
    private async Task AddPermissionClaimForRole(RoleManager<Role> roleManager, Role role, string permission)
    {
        var roleClaims = await roleManager.GetClaimsAsync(role);
        var permissionClaim = new Claim(PermissionClaimTypes.Permission, permission);

        if (!roleClaims.Any(i => i.Type == permissionClaim.Type && i.Value == permissionClaim.Value))
        {
            var result = await roleManager.AddClaimAsync(role, permissionClaim);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("Added permission claim '{ClaimValue}' to the role '{Role}'", permissionClaim.Value, role.Name);
            }
            else
            {
                _logger.LogError("Failed to add permission claim '{ClaimValue}' to the role '{Role}'", permissionClaim.Value, role.Name);
            }
        }
    }
}
