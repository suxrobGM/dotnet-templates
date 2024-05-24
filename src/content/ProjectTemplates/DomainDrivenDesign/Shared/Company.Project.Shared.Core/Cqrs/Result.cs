using System.Text.Json.Serialization;

namespace Company.Project.Shared.Cqrs;

/// <summary>
/// Represents the result of an operation.
/// </summary>
public class Result : IResult
{
    /// <summary>
    /// Gets the error message.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Error { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess => string.IsNullOrEmpty(Error);

    /// <summary>
    /// Creates a new instance of <see cref="Result"/> with <see cref="IsSuccess"/> set to <c>true</c>.
    /// </summary>
    public static Result Success() => new();
    
    /// <summary>
    /// Creates a new instance of <see cref="Result"/> with <see cref="IsSuccess"/> set to <c>false</c>.
    /// </summary>
    /// <param name="error">The error message to be returned in the Result object</param>
    public static Result Failure(string error) => new() { Error = error };
}

/// <summary>
/// Represents the result of an operation with a data payload.
/// </summary>
/// <typeparam name="T">The datatype of the payload</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Gets the data payload.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; init; }

    /// <summary>
    /// Creates a new instance of <see cref="Result{T}"/> with <see cref="IsSuccess"/> set to <c>true</c>.
    /// </summary>
    /// <param name="result">The data to be returned in the Result object</param>
    public static Result<T> Success(T result) => new() { Data = result };
    
    /// <summary>
    /// Creates a new instance of <see cref="Result"/> with <see cref="IsSuccess"/> set to <c>false</c>.
    /// </summary>
    /// <param name="error">The error message to be returned in the Result object</param>
    public new static Result<T> Failure(string error) => new() { Error = error };
}
