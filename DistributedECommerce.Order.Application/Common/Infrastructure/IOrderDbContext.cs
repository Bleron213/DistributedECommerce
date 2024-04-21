using DistributedECommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Application.Common.Infrastructure
{
    public interface IOrderDbContext
    {

        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Order> Orders { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        ChangeTracker ChangeTracker { get; }
    }
}
