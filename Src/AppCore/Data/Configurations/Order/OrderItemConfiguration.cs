using AppCore.Domains.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Order;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", "Order");

        builder.Property(oi => oi.ProductName)
            .HasMaxLength(300);

        builder.Property(oi => oi.VariantTitle)
            .HasMaxLength(300);

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(oi => oi.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(oi => oi.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(oi => oi.VendorCommissionRate)
            .HasPrecision(5, 2);

        builder.HasIndex(oi => oi.ProductVariantId)
            .HasDatabaseName("IX_OrderItems_ProductVariantId");

        builder.HasIndex(oi => oi.VendorId)
            .HasDatabaseName("IX_OrderItems_VendorId");
    }
}
