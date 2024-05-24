namespace Company.Project.Domain.Extensions;

public static class EnumerableExtensions
{
    public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
        this IEnumerable<TSource> query,
        Func<TSource, TKey> keySelector,
        bool isDescendingOrder)
    {
        return isDescendingOrder ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }
        
    public static IEnumerable<TSource> ApplyPaging<TSource>(
        this IEnumerable<TSource> query,
        int page,
        int pageSize)
    {
        if (page <= 0) // don't page if page is 0 fetch the full data
        {
            return query;
        }
        
        return query.Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}
