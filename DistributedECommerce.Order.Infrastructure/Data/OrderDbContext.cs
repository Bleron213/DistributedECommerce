using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DistributedECommerce.Orders.Infrastructure.Data
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options), IOrderDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
