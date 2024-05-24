using System.Linq.Expressions;

namespace Company.Project.Domain.Core;

public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> 
    where TEntity : IEntity<string>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }
    public Expression<Func<TEntity, object?>>? OrderBy { get; private set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public List<string> IncludeStrings { get; } = [];
    public List<FilterExpression> FilterExpressions { get; } = [];
    public int PageSize { get; private set; }
    public int Page { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    public bool IsDescending { get; private set; }
    
    protected virtual Expression<Func<TEntity, object?>> CreateOrderByExpression(string propertyName)
    {
        return i => i.Id;
    }

    protected void ApplyPaging(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(string? orderByProperty)
    {
        var parsedOrderByProperty = ParseOrderByProperty(orderByProperty);
        OrderBy = CreateOrderByExpression(parsedOrderByProperty);
    }

    private string ParseOrderByProperty(string? orderByProperty)
    {
        if (string.IsNullOrEmpty(orderByProperty))
        {
            return string.Empty;
        }

        if (orderByProperty.StartsWith('-'))
        {
            IsDescending = true;
            orderByProperty = orderByProperty[1..];
        }
        return orderByProperty.ToLower();
    }
}
