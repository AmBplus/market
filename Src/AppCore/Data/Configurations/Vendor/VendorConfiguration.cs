
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Vendor;

public class VendorConfiguration : IEntityTypeConfiguration<Domains.Vendor.Vendor>
{
    public void Configure(EntityTypeBuilder<Domains.Vendor.Vendor> builder)
    {
        builder.ToTable("Vendors", "Vendor");

        builder.Property(v => v.StoreName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.StoreSlug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Description)
            .HasMaxLength(1000);

        builder.Property(v => v.LogoUrl)
            .HasMaxLength(500);

        builder.Property(v => v.CommissionRate)
            .HasPrecision(5, 2);

        builder.Property(v => v.BankAccountNumber)
            .HasMaxLength(30);

        builder.Property(v => v.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(v => v.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Vendors_UserId");

        builder.HasIndex(v => v.StoreSlug)
            .IsUnique()
            .HasDatabaseName("IX_Vendors_StoreSlug");

        builder.HasIndex(v => v.Status)
            .HasDatabaseName("IX_Vendors_Status");

        builder.HasIndex(v => v.IsActive)
            .HasDatabaseName("IX_Vendors_IsActive");
    }
}
