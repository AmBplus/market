using AppCore.Domains.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Order;

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ToTable("OrderStatusHistories", "Order");

        builder.Property(osh => osh.Note)
            .HasMaxLength(500);

        builder.HasIndex(osh => osh.OrderId)
            .HasDatabaseName("IX_OrderStatusHistories_OrderId");

        builder.HasIndex(osh => osh.ChangedAt)
            .HasDatabaseName("IX_OrderStatusHistories_ChangedAt");
    }
}
