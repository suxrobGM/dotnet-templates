namespace Company.Project.Domain.Core;

/// <summary>
/// Specifies the comparison operator of a filter.
/// </summary>
public enum FilterOp
{
    /// <summary>
    /// Satisfied if the current value equals the specified value.
    /// </summary>
    Equals,

    /// <summary>
    /// Satisfied if the current value does not equal the specified value.
    /// </summary>
    NotEquals,

    /// <summary>
    /// Satisfied if the current value is less than the specified value.
    /// </summary>
    LessThan,

    /// <summary>
    /// Satisfied if the current value is less than or equal to the specified value.
    /// </summary>
    LessThanOrEquals,

    /// <summary>
    /// Satisfied if the current value is greater than the specified value.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Satisfied if the current value is greater than or equal to the specified value.
    /// </summary>
    GreaterThanOrEquals,
    
    /// <summary>
    /// Satisfied if the current value is not in the specified value.
    /// </summary>
    IsNull,
    
    /// <summary>
    /// Satisfied if the current value is not null.
    /// </summary>
    IsNotNull
}
