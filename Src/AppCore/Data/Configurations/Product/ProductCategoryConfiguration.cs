using AppCore.Domains.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Product;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories", "Product");

        builder.HasIndex(pc => new { pc.ProductId, pc.CategoryId })
            .IsUnique()
            .HasDatabaseName("IX_ProductCategories_ProductId_CategoryId");
    }
}
