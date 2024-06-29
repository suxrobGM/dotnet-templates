namespace Company.Project.Core.Cqrs;

public static class ResultExtensions
{
    public static bool IsError(this IResult result)
    {
        return !result.Success && !string.IsNullOrEmpty(result.Error);
    }
}
