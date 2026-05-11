using AppCore.Domains.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Shipping;

public class OrderDeliveryConfiguration : IEntityTypeConfiguration<OrderDelivery>
{
    public void Configure(EntityTypeBuilder<OrderDelivery> builder)
    {
        builder.ToTable("OrderDeliveries", "Shipping");

        builder.Property(od => od.DeliveryAddressJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(od => od.TrackingCode)
            .HasMaxLength(100);

        builder.Property(od => od.DigitalContent)
            .HasColumnType("nvarchar(max)");

        builder.Property(od => od.Note)
            .HasMaxLength(500);

        builder.HasIndex(od => od.OrderId)
            .HasDatabaseName("IX_OrderDeliveries_OrderId");

        builder.HasIndex(od => od.DeliveryMethodId)
            .HasDatabaseName("IX_OrderDeliveries_DeliveryMethodId");

        builder.HasIndex(od => od.Status)
            .HasDatabaseName("IX_OrderDeliveries_Status");
    }
}
