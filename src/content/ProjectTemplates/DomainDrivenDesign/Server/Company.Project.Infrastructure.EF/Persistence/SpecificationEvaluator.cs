using Company.Project.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Infrastructure.Persistence;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        where TEntity : class, IEntity<string>
    {
        var query = inputQuery;
        
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.FilterExpressions.Count != 0)
        {
            query = query.Where(FilterEvaluator.BuildPredicate<TEntity>(specification.FilterExpressions));
        }
        
        // Includes all expression-based includes
        query = specification.Includes.Aggregate(query,
            (current, include) => current.Include(include));

        // Include any string-based include statements
        query = specification.IncludeStrings.Aggregate(query,
            (current, include) => current.Include(include));
        
        if (specification.OrderBy is not null)
        {
            query = specification.IsDescending
                ? query.OrderByDescending(specification.OrderBy)
                : query.OrderBy(specification.OrderBy);
        }
        
        if (specification.IsPagingEnabled)
        {
            query = query.Skip((specification.Page - 1) * specification.PageSize)
                .Take(specification.PageSize);
        }
        return query;
    }
}
