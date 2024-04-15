using BoxCommerce.CustomComponent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.CustomComponent.Application.Common.Infrastructure
{
    public interface IBoxCommerceOrderDbContext
    {
        public DbSet<AuditTrail> AuditTrails { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        ChangeTracker ChangeTracker { get; }
    }
}
