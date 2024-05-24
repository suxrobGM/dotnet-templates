using Company.Project.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Project.Infrastructure.Builder;

public interface IInfrastructureBuilder : IServiceCollection
{
    IInfrastructureBuilder ConfigureIdentity(Action<IdentityBuilder> configure);
    IInfrastructureBuilder ConfigureDatabase(Action<ApplicationDbContextOptions> configure);
}
