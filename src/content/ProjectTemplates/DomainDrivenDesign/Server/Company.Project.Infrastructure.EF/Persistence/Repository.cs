using System.Linq.Expressions;
using Company.Project.Domain.Core;
using Company.Project.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using Z.BulkOperations;
using Z.EntityFramework.Plus;

namespace Company.Project.Infrastructure.Persistence;

internal class Repository<TDbContext, TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity<string>
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public Repository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
    {
        return SpecificationEvaluator.BuildQuery(_dbContext.Set<TEntity>(), specification);
    }

    public IQueryable<TEntity> Query(params string[] includes)
    {
        return ApplyIncludes(_dbContext.Set<TEntity>(), includes);
    }
    
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = default)
    {
        return predicate is null ? _dbContext.Set<TEntity>().CountAsync() : _dbContext.Set<TEntity>().CountAsync(predicate);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>().AnyAsync(predicate);
    }

    public Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool disableChangeTracking = true,
        params string[] includes)
    {
        var query = CreateQuery(disableChangeTracking, includes);
        return query.FirstOrDefaultAsync(predicate);
    }

    public Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = default,
        bool disableChangeTracking = true,
        params string[] includes)
    {
        var query = CreateQuery(disableChangeTracking, includes);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.ToListAsync();
    }

    public Task<List<TEntity>> GetListAsync(
        ISpecification<TEntity>? specification = default,
        bool disableChangeTracking = true)
    {
        var query = specification is null
            ? _dbContext.Set<TEntity>()
            : SpecificationEvaluator.BuildQuery(_dbContext.Set<TEntity>(), specification);

        if (disableChangeTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToListAsync();
    }

    public Task AddAsync(TEntity entity)
    {
        return _dbContext.Set<TEntity>().AddAsync(entity).AsTask();
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity? entity)
    {
        if (entity is null)
        {
            return;
        }

        _dbContext.Set<TEntity>().Remove(entity);
    }
    
    public void BulkAdd(IEnumerable<TEntity> entities, BulkAddOptions<TEntity>? bulkAddOptions = default)
    {
        var bulkOperationOptions = new BulkOperationOptions<TEntity>();
        
        if (bulkAddOptions?.InsertIfNotExists is not null)
        {
            bulkOperationOptions.InsertIfNotExists = bulkAddOptions.InsertIfNotExists.Value;
        }
        if (bulkAddOptions?.IncludeGraph is not null)
        {
            bulkOperationOptions.IncludeGraph = bulkAddOptions.IncludeGraph.Value;
        }
        if (bulkAddOptions?.ColumnPrimaryKeyExpression is not null)
        {
            bulkOperationOptions.ColumnPrimaryKeyExpression = bulkAddOptions.ColumnPrimaryKeyExpression;
        }
        
        _dbContext.FutureAction(_ => _dbContext.BulkInsert(entities, bulkOperationOptions));
    }
    
    public void BulkUpdate(IEnumerable<TEntity> entities, BulkUpdateOptions<TEntity>? bulkUpdateOptions = default)
    {
        var bulkOperationOptions = new BulkOperationOptions<TEntity>();
  
        if (bulkUpdateOptions?.IncludeGraph is not null)
        {
            bulkOperationOptions.IncludeGraph = bulkUpdateOptions.IncludeGraph.Value;
        }
        if (bulkUpdateOptions?.ColumnPrimaryKeyExpression is not null)
        {
            bulkOperationOptions.ColumnPrimaryKeyExpression = bulkUpdateOptions.ColumnPrimaryKeyExpression;
        }
        
        _dbContext.FutureAction(_ => _dbContext.BulkUpdate(entities, bulkOperationOptions));
    }

    public void BulkDelete(IEnumerable<TEntity> entities, BulkDeleteOptions<TEntity>? bulkDeleteOptions = default)
    {
        var bulkOperationOptions = new BulkOperationOptions<TEntity>();
        
        if (bulkDeleteOptions?.ColumnPrimaryKeyExpression is not null)
        {
            bulkOperationOptions.ColumnPrimaryKeyExpression = bulkDeleteOptions.ColumnPrimaryKeyExpression;
        }
        
        _dbContext.FutureAction(_ => _dbContext.BulkDelete(entities, bulkOperationOptions));
    }

    public void BatchDelete(Expression<Func<TEntity, bool>> predicate)
    {
        _dbContext.Set<TEntity>().Where(predicate).Delete();
    }
    
    public void BatchUpdate(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
    {
        _dbContext.Set<TEntity>().Where(predicate).Update(updateExpression);
    }

    private IQueryable<TEntity> CreateQuery(bool disableChangeTracking, params string[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        query = ApplyIncludes(query, includes);

        if (disableChangeTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params string[] includes)
    {
        return includes.Aggregate(query, (current, propertyPath) => current.Include(propertyPath));
    }
}
