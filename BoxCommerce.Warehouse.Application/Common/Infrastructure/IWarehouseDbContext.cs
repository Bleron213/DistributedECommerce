using BoxCommerce.Warehouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Application.Common.Infrastructure
{
    public interface IWarehouseDbContext
    {
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        ChangeTracker ChangeTracker { get; }
    }
}
