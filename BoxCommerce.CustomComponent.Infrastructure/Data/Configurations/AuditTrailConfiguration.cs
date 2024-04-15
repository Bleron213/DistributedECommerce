using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BoxCommerce.CustomComponent.Domain.Entities;

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

