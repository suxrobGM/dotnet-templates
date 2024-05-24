using Company.Project.Domain.Core;
using Microsoft.AspNetCore.Identity;

namespace Company.Project.Domain.Entities;

public class Role : IdentityRole, IEntity<string>
{
    public List<RoleClaim> Claims { get; set; } = [];
}
