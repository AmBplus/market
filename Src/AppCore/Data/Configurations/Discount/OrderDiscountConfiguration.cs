using AppCore.Domains.Discount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Discount;

public class OrderDiscountConfiguration : IEntityTypeConfiguration<OrderDiscount>
{
    public void Configure(EntityTypeBuilder<OrderDiscount> builder)
    {
        builder.ToTable("OrderDiscounts", "Discount");

        builder.Property(od => od.DiscountAmount)
            .HasPrecision(18, 2);

        builder.HasIndex(od => od.OrderId)
            .HasDatabaseName("IX_OrderDiscounts_OrderId");

        builder.HasIndex(od => od.DiscountId)
            .HasDatabaseName("IX_OrderDiscounts_DiscountId");
    }
}
