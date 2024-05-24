using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Company.Project.Domain.Core;
using Microsoft.AspNetCore.Identity;

namespace Company.Project.Domain.Entities;

/// <summary>
/// Join entity for user roles. Link between <see cref="User"/> and <see cref="Role"/>.
/// </summary>
public class UserRole : IdentityUserRole<string>, IEntity<string>
{
    /// <summary>
    /// Dummy property to implement <see cref="IEntity{TId}"/> and satisfy Repository type constraints.
    /// </summary>
    [NotMapped, JsonIgnore]
    public string Id { get; set; } = default!;
    
    public User? User { get; set; }
    public Role? Role { get; set; }
}
