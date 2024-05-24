using Microsoft.AspNetCore.Authorization;

namespace Company.Project.API.Authorization;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
    
    public string Permission { get; }
}
