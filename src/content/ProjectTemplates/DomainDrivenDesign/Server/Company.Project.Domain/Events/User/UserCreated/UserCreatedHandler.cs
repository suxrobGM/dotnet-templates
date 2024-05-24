using Company.Project.Domain.Core;
using Microsoft.Extensions.Logging;

namespace Company.Project.Domain.Events;

internal class UserCreatedHandler : IDomainEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedHandler> _logger;
   
    public UserCreatedHandler(ILogger<UserCreatedHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User created with id {User} and email {Email}", notification.UserId, notification.Email);
        return Task.CompletedTask;
    }
}
