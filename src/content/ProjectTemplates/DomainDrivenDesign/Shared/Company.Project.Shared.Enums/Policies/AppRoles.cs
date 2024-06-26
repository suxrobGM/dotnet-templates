namespace Company.Project.Shared.Policies;

public static class AppRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";

    public static IEnumerable<string> GetRoleNames()
    {
        yield return SuperAdmin;
        yield return Admin;
    }
}
