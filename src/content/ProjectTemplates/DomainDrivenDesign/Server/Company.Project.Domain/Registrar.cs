using Company.Project.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Project.Domain;

public static class Registrar
{
    public static IServiceCollection AddDomainLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Registrar).Assembly));
        services.AddScoped<AuthenticatedUserData>();
        return services;
    }
}
