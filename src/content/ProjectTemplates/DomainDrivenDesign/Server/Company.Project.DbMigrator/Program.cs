using Company.Project.DbMigrator.Data;
using Company.Project.Domain;
using Company.Project.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDomainLayer();
        services.AddInfrastructureLayer(ctx.Configuration);
        services.AddHostedService<SeedData>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
