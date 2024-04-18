using BoxCommerce.Warehouse.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Infrastructure.Data.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Code).HasMaxLength(500);
            builder.Property(x => x.SerialCode).HasMaxLength(500);
            builder.HasIndex(x => x.SerialCode).IsUnique();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Code).HasMaxLength(500);
            builder.Property(x => x.SerialCode).HasMaxLength(500);
            builder.Property(x => x.OrderNumber).HasMaxLength(500);

            builder.HasIndex(x => x.SerialCode).IsUnique();

        }
    }
}
