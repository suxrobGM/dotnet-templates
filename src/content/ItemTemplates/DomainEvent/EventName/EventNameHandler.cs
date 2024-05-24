using %NAMESPACE%.Core;
using Microsoft.Extensions.Logging;

namespace %NAMESPACE%.Events;

internal class EventNameHandler : IDomainEventHandler<EventNameEvent>
{
    private readonly ILogger<EventNameHandler> _logger;
   
    public EventNameHandler(ILogger<EventNameHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(EventNameEvent @event, CancellationToken ct)
    {
        _logger.LogInformation("Handled the domain event 'EventName'");
        return Task.CompletedTask;
    }
}
