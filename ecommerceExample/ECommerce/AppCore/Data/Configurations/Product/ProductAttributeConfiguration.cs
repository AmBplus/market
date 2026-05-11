using ECommerce.AppCore.Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Product;

public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable("ProductAttributes", "Product");

        builder.Property(pa => pa.Key)
            .HasMaxLength(200);

        builder.Property(pa => pa.Value)
            .HasMaxLength(500);

        builder.Property(pa => pa.Group)
            .HasMaxLength(100);

        builder.HasIndex(pa => new { pa.ProductId, pa.Key })
            .HasDatabaseName("IX_ProductAttributes_ProductId_Key");
    }
}
