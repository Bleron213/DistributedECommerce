using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Domain.Entities;
using Newtonsoft.Json;
using DistributedECommerce.Orders.Domain.Entities.Interfaces;
using DistributedECommerce.Orders.Domain.Entities.Attributes;

namespace DistributedECommerce.Orders.Infrastructure.Data.Interceptors;

public class AuditTrailInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _user;

    public AuditTrailInterceptor(
        ICurrentUserService user)
    {
        _user = user;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            if (!entry.Entity.GetType().IsAssignableTo(typeof(IAuditable)))
                continue;

            if (entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            var audit = new AuditTrail
            {
                AuditId = Guid.NewGuid(),
                AffectedEntity = entry.Entity.GetType().Name,
                AuditDate = DateTimeOffset.UtcNow,
                CreatedBy = _user.UserId.ToString(),
            };

            Dictionary<string, object> oldValues = new();
            Dictionary<string, object> newValues = new();
            foreach (var property in entry.Properties)
            {
                var isAuditTrackeableDefined = Attribute.IsDefined(property.Metadata.PropertyInfo!, typeof(TrackPropertyAttribute));
                var isPk = property.Metadata.IsPrimaryKey();
                if (!isAuditTrackeableDefined && !property.Metadata.IsPrimaryKey())
                    continue;

                var propertyName = property.Metadata.PropertyInfo!.Name;

                if (property.Metadata.IsPrimaryKey())
                    audit.AffectedEntityId = (Guid)property.CurrentValue!;

                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            audit.AuditType = Domain.Enums.AuditType.Added;
                            newValues[propertyName] = property.CurrentValue!;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            audit.AuditType = Domain.Enums.AuditType.Updated;
                            oldValues[propertyName] = property.CurrentValue!;
                            newValues[propertyName] = property.CurrentValue!;
                            break;
                        }
                }

                audit.NewValues = JsonConvert.SerializeObject(newValues);
                audit.OldValues = JsonConvert.SerializeObject(oldValues);
            }
            context.Add(audit);
        }

    }
}
