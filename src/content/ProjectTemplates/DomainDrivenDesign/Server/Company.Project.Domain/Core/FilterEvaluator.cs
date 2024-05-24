using System.Linq.Expressions;

namespace Company.Project.Domain.Core;

public static class FilterEvaluator
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(IEnumerable<FilterExpression> filterExpressions)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combined = null;

        foreach (var filterExpression in filterExpressions)
        {
            var property = Expression.Property(parameter, filterExpression.Property);
            var constantExpr = BuildConstantExpression(filterExpression, property);

            Expression comparison = filterExpression.Operator switch
            {
                FilterOp.Equals => Expression.Equal(property, constantExpr!),
                FilterOp.NotEquals => Expression.NotEqual(property, constantExpr!),
                FilterOp.LessThan => Expression.LessThan(property, constantExpr!),
                FilterOp.LessThanOrEquals => Expression.LessThanOrEqual(property, constantExpr!),
                FilterOp.GreaterThan => Expression.GreaterThan(property, constantExpr!),
                FilterOp.GreaterThanOrEquals => Expression.GreaterThanOrEqual(property, constantExpr!),
                FilterOp.IsNull => Expression.Equal(property, Expression.Constant(null)),
                FilterOp.IsNotNull => Expression.NotEqual(property, Expression.Constant(null)),
                _ => throw new NotSupportedException($"Operator {filterExpression.Operator} is not supported"),
            };

            combined = combined is null ? comparison : Expression.AndAlso(combined, comparison);
        }

        if (combined is null)
        {
            throw new InvalidOperationException("No filter expression provided");
        }

        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }

    private static ConstantExpression? BuildConstantExpression(FilterExpression filterExpression, MemberExpression property)
    {
        // Create constant expression only for operators that need a value
        if (filterExpression.Operator is FilterOp.IsNull or FilterOp.IsNotNull)
        {
            return null;
        }
            
        // Handle nullable enum conversion
        var constant = property.Type.IsGenericType &&
                       property.Type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                       property.Type.GetGenericArguments().First().IsEnum
            ? ConvertToNullableEnum(filterExpression.Value, property.Type)
            : Convert.ChangeType(filterExpression.Value, property.Type);
        
        return Expression.Constant(constant, property.Type);
    }
    
    private static object? ConvertToNullableEnum(object? value, Type targetType)
    {
        if (value is null)
        {
            return null;
        }
        
        var enumType = targetType.GetGenericArguments().First();
        return Enum.ToObject(enumType, value);
    }
}
