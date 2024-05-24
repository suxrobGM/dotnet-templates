﻿using System.Linq.Expressions;
using Company.Project.Domain.Core;

namespace Company.Project.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplySpecification<TEntity>(
        this IQueryable<TEntity> queryable,
        ISpecification<TEntity> specification)
        where TEntity : class, IEntity<string>
    {
        var query = queryable;
        if (specification.Criteria is not null)
        {
            query = queryable.Where(specification.Criteria);
        }

        if (specification.OrderBy is not null)
        {
            query = specification.IsDescending ? 
                query.OrderByDescending(specification.OrderBy)
                : query.OrderBy(specification.OrderBy);
        }
        
        if (specification.IsPagingEnabled)
        {
            query = query.Skip((specification.Page - 1) * specification.PageSize)
                .Take(specification.PageSize);
        }

        return query;
    }
    
    public static IQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TKey>> keySelector,
        bool isDescendingOrder)
    {
        return isDescendingOrder ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }
        
    public static IQueryable<TSource> ApplyPaging<TSource>(
        this IQueryable<TSource> query,
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
