namespace Company.Project.Domain.Core;

public class AuditableEntity : Entity, IAuditableEntity<string>
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
