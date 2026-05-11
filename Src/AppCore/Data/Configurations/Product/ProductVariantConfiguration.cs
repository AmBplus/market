using AppCore.Domains.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Product;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants", "Product");

        builder.Property(pv => pv.VariantCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pv => pv.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pv => pv.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(pv => pv.VariantAttributesJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(pv => pv.CostPrice)
            .HasPrecision(18, 2);

        builder.Property(pv => pv.DefaultPrice)
            .HasPrecision(18, 2);

        builder.Property(pv => pv.WeightGrams)
            .HasPrecision(10, 2);

        builder.Property(pv => pv.IsActive)
            .HasDefaultValue(true);

        builder.Property(pv => pv.IsDefault)
            .HasDefaultValue(false);

        builder.Property(pv => pv.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(pv => pv.VariantCode)
            .IsUnique()
            .HasDatabaseName("IX_ProductVariants_VariantCode");

        builder.HasIndex(pv => pv.Sku)
            .IsUnique()
            .HasDatabaseName("IX_ProductVariants_Sku");

        builder.HasIndex(pv => pv.ProductId)
            .HasDatabaseName("IX_ProductVariants_ProductId");

        builder.HasIndex(pv => pv.VendorId)
            .HasDatabaseName("IX_ProductVariants_VendorId");

        builder.HasIndex(pv => new { pv.ProductId, pv.IsActive })
            .HasDatabaseName("IX_ProductVariants_ProductId_IsActive");

        builder.HasQueryFilter(pv => !pv.IsDeleted);
    }
}
