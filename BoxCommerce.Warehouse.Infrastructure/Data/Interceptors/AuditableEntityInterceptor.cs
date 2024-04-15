using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Domain.Entities.Attributes;
using BoxCommerce.Warehouse.Domain.Enums;
using BoxCommerce.Warehouse.Domain.Entities.Interfaces;
using BoxCommerce.Warehouse.Domain.Entities;

namespace BoxCommerce.Warehouse.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _user;

    public AuditableEntityInterceptor(
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

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _user.UserId.ToString();
                entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = _user.UserId.ToString();
                entry.Entity.LastModifiedOn = DateTimeOffset.UtcNow;
            }
        }
    }
}

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
                            audit.AuditType = AuditType.Added;
                            newValues[propertyName] = property.CurrentValue!;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            audit.AuditType = AuditType.Updated;
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

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
