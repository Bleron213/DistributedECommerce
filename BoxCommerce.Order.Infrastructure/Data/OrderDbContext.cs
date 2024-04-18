using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BoxCommerce.Orders.Infrastructure.Data
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
