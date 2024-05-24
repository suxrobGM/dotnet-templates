using System.Collections;
using Company.Project.Domain.Core;
using Company.Project.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Infrastructure.Persistence;

internal class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly Hashtable _repositories = new();

    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity<string>
    {
        var type = typeof(TEntity).Name;
        
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new Repository<TDbContext, TEntity>(_dbContext);
            _repositories.Add(type, repositoryInstance);
        }
        
        if (_repositories[type] is not Repository<TDbContext, TEntity> repository)
        {
            throw new InvalidOperationException("Could not create a repository");
        }
        
        return repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _dbContext.ExecuteFutureAction();
        var changes = await _dbContext.SaveChangesAsync(cancellationToken);
        return changes;
    }
    
    public void Dispose()
    {
        if (_dbContext is IDisposable dbContextDisposable)
        {
            dbContextDisposable.Dispose();
        }
        else
        {
            _ = _dbContext.DisposeAsync().AsTask();
        
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}
