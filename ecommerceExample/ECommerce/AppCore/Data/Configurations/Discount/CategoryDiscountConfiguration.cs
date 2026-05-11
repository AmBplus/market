using ECommerce.AppCore.Domain.Discount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Discount;

public class CategoryDiscountConfiguration : IEntityTypeConfiguration<CategoryDiscount>
{
    public void Configure(EntityTypeBuilder<CategoryDiscount> builder)
    {
        builder.ToTable("CategoryDiscounts", "Discount");

        builder.HasIndex(cd => new { cd.DiscountId, cd.CategoryId })
            .IsUnique()
            .HasDatabaseName("IX_CategoryDiscounts_DiscountId_CategoryId");
    }
}
