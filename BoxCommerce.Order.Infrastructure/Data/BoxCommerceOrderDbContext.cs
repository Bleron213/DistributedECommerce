using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Orders.Infrastructure.Data
{
    public class BoxCommerceOrderDbContext(DbContextOptions<BoxCommerceOrderDbContext> options) : DbContext(options), IBoxCommerceOrderDbContext
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
