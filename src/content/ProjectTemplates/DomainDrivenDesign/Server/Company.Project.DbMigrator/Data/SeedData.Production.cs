namespace Company.Project.DbMigrator.Data;

/// <summary>
/// The partial class for seeding the database with initial data in the Production environment.
/// </summary>
public partial class SeedData
{
    /// <summary>
    /// Seeds the database with initial data in the Production environment.
    /// </summary>
    private async Task SeedDatabaseAsync_Production()
    {
        _logger.LogInformation("[Production] Seeding the database...");
        await AddAppRolesAsync();
        await AddSuperAdminAsync();
        _logger.LogInformation("Successfully seeded the database");
    }
}
