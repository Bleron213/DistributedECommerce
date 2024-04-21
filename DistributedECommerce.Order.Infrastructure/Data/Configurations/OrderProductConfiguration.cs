using DistributedECommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DistributedECommerce.Orders.Infrastructure.Data.Configurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderedProducts)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
