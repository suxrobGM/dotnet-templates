using Company.Project.Domain.Core;

namespace Company.Project.Domain.Persistence;

/// <summary>
/// Generic Unit of Work
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Creates a new repository instance for the specified entity if it was not in the cache
    /// </summary>
    /// <typeparam name="TEntity">Entity class</typeparam>
    /// <returns>An instance of generic repository for the specified entity</returns>
    IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity<string>;
    
    /// <summary>
    /// Save changes to database
    /// </summary>
    /// <returns>Number of rows modified after save changes.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
