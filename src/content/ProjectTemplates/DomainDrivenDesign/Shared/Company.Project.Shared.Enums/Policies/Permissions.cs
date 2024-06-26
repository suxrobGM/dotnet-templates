using System.Reflection;

namespace Company.Project.Shared.Policies;

public static class Permissions
{
    public static IEnumerable<string> AllPermissions { get; }
    
    static Permissions()
    {
        AllPermissions = GetAllPermissions();
    }
    
    public const string CanAddUser = $"{nameof(Permissions)}.AddUser";
    public const string CanDeleteUser = $"{nameof(Permissions)}.DeleteUser";
    public const string CanUpdateUser = $"{nameof(Permissions)}.UpdateUser";
    
    private static IEnumerable<string> GetAllPermissions()
    {
        var fields = typeof(Permissions).GetFields(BindingFlags.Public | BindingFlags.Static);
        return fields.Select(f => f.GetValue(null)!.ToString()!);
    }
}
