namespace Company.Project.Core.Cqrs;

/// <summary>
/// Represents the result of an operation
/// </summary>
public interface IResult
{   
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    bool IsSuccess { get; }
    
    /// <summary>
    /// Gets the error message.
    /// </summary>
    string? Error { get; init; }
}
