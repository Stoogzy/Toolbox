using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Toolbox.Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? context = eventData.Context;

        if (context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // This logic looks for entities that implement a specific 'Auditable' interface
        foreach (var entry in context.ChangeTracker.Entries<dynamic>())
        {
            // We'll refine this once we create your Domain entities
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.UtcNow;
                entry.Entity.CreatedBy = "System/CurrentUserId"; // Logic for current user goes here.
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = "System/CurrentUserId";
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
