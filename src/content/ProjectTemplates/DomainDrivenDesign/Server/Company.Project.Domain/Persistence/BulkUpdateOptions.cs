using System.Linq.Expressions;
using Company.Project.Domain.Core;

namespace Company.Project.Domain.Persistence;

public class BulkUpdateOptions<TEntity> where TEntity : class, IEntity<string>
{
    /// <summary>
    /// This option allows updating entities along with all related entities found in the entity graph, maintaining the data relationships.
    /// </summary>
    public bool? IncludeGraph { get; set; }
    
    /// <summary>
    /// This option enables you to specify a subset of columns to update by using an expression.
    /// </summary>
    public Expression<Func<TEntity,object>>? ColumnInputExpression { get; set; }
    
    /// <summary>
    /// This option allows you to use a custom key to check for pre-existing entities.
    /// </summary>
    public Expression<Func<TEntity,object>>? ColumnPrimaryKeyExpression { get; set; }
}
