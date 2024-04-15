using BoxCommerce.CustomComponent.Application.Common.Infrastructure;
using BoxCommerce.CustomComponent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BoxCommerce.CustomComponent.Infrastructure.Data
{
    public class BoxCommerceOrderDbContext(DbContextOptions<BoxCommerceOrderDbContext> options) : DbContext(options), IBoxCommerceOrderDbContext
    {
        public DbSet<AuditTrail> AuditTrails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
