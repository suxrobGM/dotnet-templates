using Company.Project.Domain.Core;
using Company.Project.Domain.Identity;
using Company.Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Z.EntityFramework.Extensions;

namespace Company.Project.Infrastructure.Interceptors;

public class AuditingEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly AuthenticatedUserData? _authenticatedUserData;

    static AuditingEntitiesInterceptor()
    {
        RegisterBulkOperationEvents();
    }
    
    public AuditingEntitiesInterceptor(AuthenticatedUserData? authenticatedUserData = null)
    {
        _authenticatedUserData = authenticatedUserData;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var userId = _authenticatedUserData?.UserId;
        var auditableEntities = context.ChangeTracker.Entries<IAuditableEntity<string>>();
        
        foreach (var entry in auditableEntities)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedDate = DateTime.UtcNow;
            } 

            if (entry.State is EntityState.Modified || HasChangedOwnedEntities(entry))
            {
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }
        }
    }
    
    private static bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Modified);
    }
    
    private static void RegisterBulkOperationEvents()
    {
        EntityFrameworkManager.PreBulkInsert = (context, obj) =>
        {
            if (obj is IEnumerable<IAuditableEntity<string>> auditableEntities && 
                context is ApplicationDbContext applicationDbContext)
            {
                var userId = applicationDbContext.GetService<AuthenticatedUserData>()?.UserId;
                foreach (var auditableEntity in auditableEntities)
                {
                    auditableEntity.CreatedBy = userId;
                }
            }
        };
    }
}
