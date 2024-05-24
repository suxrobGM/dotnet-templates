using System.Linq.Expressions;
using Company.Project.Domain.Core;

namespace Company.Project.Domain.Persistence;

public class BulkDeleteOptions<TEntity> where TEntity : class, IEntity<string>
{
    /// <summary>
    /// This option allows the usage of a custom key to verify the existence of entities.
    /// </summary>
    public Expression<Func<TEntity,object>>? ColumnPrimaryKeyExpression { get; set; }
}
