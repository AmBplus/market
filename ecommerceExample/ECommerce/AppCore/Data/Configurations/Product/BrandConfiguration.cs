using ECommerce.AppCore.Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Product;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", "Product");

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.LogoUrl)
            .HasMaxLength(500);

        builder.Property(b => b.Description)
            .HasMaxLength(2000);

        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(b => b.Name)
            .IsUnique()
            .HasDatabaseName("IX_Brands_Name");

        builder.HasIndex(b => b.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Brands_Slug");

        builder.HasMany(b => b.Products)
            .WithOne(p => p.Brand)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
