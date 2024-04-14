using BoxCommerce.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
{
    public void Configure(EntityTypeBuilder<AuditTrail> builder)
    {
        builder.HasKey(x => x.AuditId);

        builder.Property(x => x.AuditId).ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedBy).HasMaxLength(100);
        builder.Property(x => x.AffectedEntity).HasMaxLength(50);
        builder.Property(x => x.OldValues);
        builder.Property(x => x.NewValues);
    }
}

