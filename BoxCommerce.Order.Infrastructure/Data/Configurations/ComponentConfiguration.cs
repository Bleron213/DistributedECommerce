using BoxCommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoxCommerce.Orders.Infrastructure.Data.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Component> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(500);
            builder.Property(x => x.ComponentCode).HasMaxLength(500);
        }
    }
}
