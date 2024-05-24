using Company.Project.Domain.Core;

namespace Company.Project.Domain.Events;

internal class UserCreatedEvent : IDomainEvent
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
}
