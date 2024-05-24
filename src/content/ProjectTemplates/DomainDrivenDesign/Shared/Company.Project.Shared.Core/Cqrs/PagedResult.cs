namespace Company.Project.Shared.Cqrs;

/// <summary>
/// Represents a paged result. This class is used to return a paged list of items.
/// </summary>
/// <typeparam name="T">The datatype of paged items</typeparam>
public class PagedResult<T> : Result<IEnumerable<T>>
{
    public PagedResult() : this(null, 0, 0)
    {
    }

    public PagedResult(IEnumerable<T>? data, int totalItems, int pageSize)
    {
        Data = data;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    /// <summary>
    /// Gets the total number of items.
    /// This is the total number of items in the entire collection, not just the items returned in this PagedResult object.
    /// </summary>
    public int TotalItems { get; set; }
    
    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="PagedResult{T}"/> with <see cref="IsSuccess"/> set to <c>true</c>.
    /// </summary>
    /// <param name="items">The data to be returned in the PagedResult object</param>
    /// <param name="totalItems">The total number of items</param>
    /// <param name="totalPages">The total number of pages</param>
    public static PagedResult<T> Success(IEnumerable<T>? items, int totalItems, int totalPages) =>
        new(items, totalItems, totalPages);

    /// <summary>
    /// Creates a new instance of <see cref="PagedResult{T}"/> with <see cref="IsSuccess"/> set to <c>false</c> and an error message.
    /// </summary>
    /// <param name="error">The error message to be returned in the PagedResult object</param>
    public new static PagedResult<T> Failure(string error) =>
        new(null, 0, 0) { Error = error };
}
