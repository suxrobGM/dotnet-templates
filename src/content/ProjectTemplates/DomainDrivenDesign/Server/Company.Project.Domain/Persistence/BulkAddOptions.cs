using System.Linq.Expressions;
using Company.Project.Domain.Core;

namespace Company.Project.Domain.Persistence;

public class BulkAddOptions<TEntity> where TEntity : class, IEntity<string>
{
    /// <summary>
    /// This option allows to optimize performance by not returning outputting values such as identity values.
    /// </summary>
    public bool? AutoMapOutputDirection { get; set; }
    
    /// <summary>
    /// This option ensures only new entities that don't already exist in the database are inserted.
    /// </summary>
    public bool? InsertIfNotExists { get; set; }
    
    /// <summary>
    /// This option allows you to keep the identity value of your entity.
    /// </summary>
    public bool? InsertKeepIdentity { get; set; }

    /// <summary>
    /// This option enables the insertion of entities along with all related entities found in the entity graph, maintaining the relationships.
    /// </summary>
    public bool? IncludeGraph { get; set; }
    
    /// <summary>
    /// This option allows the usage of a custom key to verify the existence of entities.
    /// </summary>
    public Expression<Func<TEntity,object>>? ColumnPrimaryKeyExpression { get; set; }
}
