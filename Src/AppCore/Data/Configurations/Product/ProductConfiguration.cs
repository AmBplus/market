
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Product;

public class ProductConfiguration : IEntityTypeConfiguration<Domains.Product.Product>
{
    public void Configure(EntityTypeBuilder<Domains.Product.Product> builder)
    {
        builder.ToTable("Products", "Product");

        builder.Property(p => p.ProductCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.ShortDescription)
            .HasMaxLength(500);

        builder.Property(p => p.ShippingConstraintsJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.MetaTitle)
            .HasMaxLength(200);

        builder.Property(p => p.MetaDescription)
            .HasMaxLength(500);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        builder.Property(p => p.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(p => p.ProductCode)
            .IsUnique()
            .HasDatabaseName("IX_Products_ProductCode");

        builder.HasIndex(p => p.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Products_Slug");

        builder.HasIndex(p => p.BrandId)
            .HasDatabaseName("IX_Products_BrandId");

        builder.HasIndex(p => p.VendorId)
            .HasDatabaseName("IX_Products_VendorId");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");

        builder.HasIndex(p => p.IsFeatured)
            .HasDatabaseName("IX_Products_IsFeatured");

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasMany(p => p.Variants)
            .WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductCategories)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Images)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Attributes)
            .WithOne(pa => pa.Product)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
