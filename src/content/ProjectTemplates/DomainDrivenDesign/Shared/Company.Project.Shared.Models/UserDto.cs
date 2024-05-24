namespace Company.Project.Shared.Models;

public record UserDto
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}
