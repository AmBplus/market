
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Order;

public class OrderConfiguration : IEntityTypeConfiguration<Domains.Order.Order>
{
    public void Configure(EntityTypeBuilder<Domains.Order.Order> builder)
    {
        builder.ToTable("Orders", "Order");

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.SubTotal)
            .HasPrecision(18, 2);

        builder.Property(o => o.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(o => o.ShippingCost)
            .HasPrecision(18, 2);

        builder.Property(o => o.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(o => o.CurrencyRate)
            .HasPrecision(18, 6);

        builder.Property(o => o.CustomerNote)
            .HasMaxLength(500);

        builder.Property(o => o.AdminNote)
            .HasMaxLength(500);

        builder.Property(o => o.ShippingAddressJson)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(o => o.OrderNumber)
            .IsUnique()
            .HasDatabaseName("IX_Orders_OrderNumber");

        builder.HasIndex(o => o.UserId)
            .HasDatabaseName("IX_Orders_UserId");

        builder.HasIndex(o => o.Status)
            .HasDatabaseName("IX_Orders_Status");

        builder.HasIndex(o => o.CreatedAt)
            .HasDatabaseName("IX_Orders_CreatedAt");

        builder.HasIndex(o => new { o.UserId, o.Status })
            .HasDatabaseName("IX_Orders_UserId_Status");

        builder.HasIndex(o => new { o.Status, o.CreatedAt })
            .HasDatabaseName("IX_Orders_Status_CreatedAt");
    }
}
