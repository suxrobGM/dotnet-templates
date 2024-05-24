namespace Company.Project.Domain.Core;

public record FilterExpression
{
    public required string Property { get; set; }
    public required FilterOp Operator { get; set; }
    public required object Value { get; set; }
}
