using Microsoft.AspNetCore.Identity;

namespace Company.Project.Domain.Entities;

public class RoleClaim : IdentityRoleClaim<string>
{
    public Role? Role { get; set; }
}
