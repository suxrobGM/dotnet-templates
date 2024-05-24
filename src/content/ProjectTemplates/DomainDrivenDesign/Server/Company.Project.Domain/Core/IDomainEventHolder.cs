namespace Company.Project.Domain.Core;

public interface IDomainEventHolder
{
    List<IDomainEvent> DomainEvents { get; }
}
