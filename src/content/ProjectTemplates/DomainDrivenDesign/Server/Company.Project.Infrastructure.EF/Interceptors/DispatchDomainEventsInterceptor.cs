using Company.Project.Domain.Core;
using Company.Project.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Extensions;

namespace Company.Project.Infrastructure.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;
    private readonly ILogger<DispatchDomainEventsInterceptor> _logger;

    static DispatchDomainEventsInterceptor()
    {
        RegisterBulkOperationEvents();
    }

    public DispatchDomainEventsInterceptor(
        IMediator mediator,
        ILogger<DispatchDomainEventsInterceptor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        if (context is not null)
        {
            var entities = context.ChangeTracker.Entries<Entity>().Select(i => i.Entity);
            DispatchDomainEvents(entities, _mediator, _logger).GetAwaiter().GetResult();
        }
        
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var response = await base.SavingChangesAsync(eventData, result, cancellationToken);
        var context = eventData.Context;

        if (context is not null)
        {
            var entities = context.ChangeTracker.Entries<Entity>().Select(i => i.Entity).ToArray();
            await DispatchDomainEvents(entities, _mediator, _logger, cancellationToken);
        }
        
        return response;
    }
    
    private static async Task DispatchDomainEvents(
        IEnumerable<IDomainEventHolder> entities,
        IMediator mediator,
        ILogger? logger = default,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
                logger?.LogInformation("Dispatched domain event '{EventName}'", domainEvent.GetType().Name);
            }
            
            entity.DomainEvents.Clear();
        }
    }
    
    private static void RegisterBulkOperationEvents()
    {
        EntityFrameworkManager.PreBulkInsert = (context, obj) =>
        {
            if (obj is IEnumerable<IDomainEventHolder> entities && 
                context is ApplicationDbContext applicationDbContext)
            {
                var mediator = applicationDbContext.GetRequiredService<IMediator>();
                var logger = applicationDbContext.GetRequiredService<ILogger<DispatchDomainEventsInterceptor>>();
                DispatchDomainEvents(entities, mediator, logger).GetAwaiter().GetResult();
            }
        };
    }
}
