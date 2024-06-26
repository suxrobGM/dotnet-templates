namespace Company.Project.Core.Cqrs;

public class PagedQuery
{
    public PagedQuery(
        string? orderBy = null,
        int page = 1,
        int pageSize = 10)
    {
        OrderBy = orderBy;
        Page = page;
        PageSize = pageSize;
    }
    
    public string? OrderBy { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    
    /// <summary>
    /// Converts the PagedQuery object into a dictionary.
    /// </summary>
    /// <returns>A dictionary with keys "page", "pageSize", and optionally "orderBy" if it is not null or empty.</returns>
    public virtual IDictionary<string, string> ToDictionary()
    {
        var queryDict = new Dictionary<string, string>
        {
            {"page", Page.ToString()},
            {"pageSize", PageSize.ToString()}
        };

        if (!string.IsNullOrEmpty(OrderBy))
        {
            queryDict.Add("orderBy", OrderBy);
        }

        return queryDict;
    }
}
