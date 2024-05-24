using %NAMESPACE%.Core;

namespace %NAMESPACE%.Events;

internal class EventNameEvent : IDomainEvent
{
    public string? Id { get; set; }
}
