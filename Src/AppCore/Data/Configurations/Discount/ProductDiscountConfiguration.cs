using AppCore.Domains.Discount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Discount;

public class ProductDiscountConfiguration : IEntityTypeConfiguration<ProductDiscount>
{
    public void Configure(EntityTypeBuilder<ProductDiscount> builder)
    {
        builder.ToTable("ProductDiscounts", "Discount");

        builder.HasIndex(pd => new { pd.DiscountId, pd.ProductId, pd.ProductVariantId })
            .IsUnique()
            .HasDatabaseName("IX_ProductDiscounts_DiscountId_ProductId_ProductVariantId");
    }
}
