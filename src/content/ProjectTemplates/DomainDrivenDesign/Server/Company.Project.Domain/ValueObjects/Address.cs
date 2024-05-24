using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Project.Domain.ValueObjects;

[ComplexType]
public record Address
{
    // Temporary workaround for handling null values
    public static readonly Address NullAddress = new()
    {
        Line1 = "NULL",
        Line2 = "NULL",
        City = "NULL",
        ZipCode = "NULL",
        Region = "NULL",
        Country = "NULL"
    };
    
    public string? Line1 { get; init; }
    public string? Line2 { get; init; }
    public string? City { get; init; }
    public string? ZipCode { get; init; }
    public string? Region { get; init; }
    public string? Country { get; init; }

    public bool IsNull() => this == NullAddress;
    public bool IsNotNull() => !IsNull();
}
