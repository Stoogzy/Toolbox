using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Toolbox.Core.Common;

namespace Toolbox.Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
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

    public void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        // Filter the tracker to only entities implementing IAuditableEntity.
        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.UtcNow;
                entry.Entity.CreatedBy = "System";
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = "System";
            }
        }
    }
}
