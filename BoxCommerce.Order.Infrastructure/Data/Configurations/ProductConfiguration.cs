using BoxCommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoxCommerce.Orders.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Code).HasMaxLength(500);
            builder.Property(x => x.Name).HasMaxLength(1000);
        }
    }
}
