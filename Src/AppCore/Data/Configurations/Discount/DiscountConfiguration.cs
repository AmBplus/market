using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Discount;

public class DiscountConfiguration : IEntityTypeConfiguration<Domains.Discount.Discount>
{
    public void Configure(EntityTypeBuilder<Domains.Discount.Discount> builder)
    {
        builder.ToTable("Discounts", "Discount");

        builder.Property(d => d.Code).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Name).HasMaxLength(200);
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.Value).HasPrecision(18, 2);
        builder.Property(d => d.MaxDiscountAmount).HasPrecision(18, 2);
        builder.Property(d => d.MinOrderAmount).HasPrecision(18, 2);
        builder.Property(d => d.IsActive).HasDefaultValue(true);

        builder.HasIndex(d => d.Code).IsUnique().HasDatabaseName("IX_Discounts_Code");
        builder.HasIndex(d => new { d.IsActive, d.StartedAt, d.EndedAt })
            .HasDatabaseName("IX_Discounts_IsActive_StartedAt_EndedAt");
    }
}
