using MediatR;

namespace Company.Project.Domain.Core;

public interface IDomainEventHandler<in T> : INotificationHandler<T> where T : IDomainEvent
{
}
