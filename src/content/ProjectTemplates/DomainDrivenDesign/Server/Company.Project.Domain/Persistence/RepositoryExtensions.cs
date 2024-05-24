using Company.Project.Domain.Core;

namespace Company.Project.Domain.Persistence;

/// <summary>
/// Repository extension methods.
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    /// Gets an entity object by ID.
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <param name="id">Entity ID</param>
    /// <param name="disableChangeTracking">Flag to specify if change tracking should be disabled (default: true)</param>
    /// <param name="includes">List of child entities to load</param>
    /// <returns>The entity object that satisfies the predicate, or null if not found</returns>
    public static Task<TEntity?> GetByIdAsync<TEntity>(
        this IRepository<TEntity> repository,
        string? id,
        bool disableChangeTracking = true,
        params string[] includes) where TEntity : class, IEntity<string>
    {
        return string.IsNullOrEmpty(id) ? 
            Task.FromResult<TEntity?>(default) : 
            repository.GetAsync(e => e.Id == id, disableChangeTracking, includes);
    }
    
    /// <summary>
    /// Deletes an entity object by ID.
    /// </summary>
    /// <param name="repository">Repository</param>
    /// <param name="id">Entity ID</param>
    public static async Task DeleteByIdAsync<TEntity>(
        this IRepository<TEntity> repository,
        string? id) where TEntity : class, IEntity<string>
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }
        
        var entity = await repository.GetByIdAsync(id);
        repository.Delete(entity);
    }
}
