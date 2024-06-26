namespace Company.Project.Shared.Policies;

public static class AppRolePermissions
{
    public static IEnumerable<string> SuperAdmin => Permissions.AllPermissions;
    public static IEnumerable<string> Admin => Permissions.AllPermissions;
    
    public static IEnumerable<string> GetRolePermissions(string roleName)
    {
        return roleName switch
        {
            AppRoles.SuperAdmin => SuperAdmin,
            AppRoles.Admin => Admin,
            _ => []
        };
    }
}
