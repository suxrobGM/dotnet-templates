namespace Company.Project.Core.Cqrs;

public static class ResultExtensions
{
    public static bool IsError(this IResult result)
    {
        return !result.IsSuccess && !string.IsNullOrEmpty(result.Error);
    }
}
