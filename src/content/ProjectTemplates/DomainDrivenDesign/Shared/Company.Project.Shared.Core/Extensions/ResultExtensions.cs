namespace Company.Project.Shared.Cqrs;

public static class ResultExtensions
{
    public static bool IsError(this IResult result)
    {
        return !result.IsSuccess && !string.IsNullOrEmpty(result.Error);
    }
}
