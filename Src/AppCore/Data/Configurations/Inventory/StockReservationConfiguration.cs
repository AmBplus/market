using AppCore.Domains.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Inventory;

public class StockReservationConfiguration : IEntityTypeConfiguration<StockReservation>
{
    public void Configure(EntityTypeBuilder<StockReservation> builder)
    {
        builder.ToTable("StockReservations", "Inventory");

        builder.HasIndex(sr => sr.OrderId)
            .HasDatabaseName("IX_StockReservations_OrderId");

        builder.HasIndex(sr => new { sr.ProductVariantId, sr.WarehouseId, sr.IsReleased })
            .HasDatabaseName("IX_StockReservations_ProductVariantId_WarehouseId_IsReleased");

        builder.HasIndex(sr => new { sr.ExpiresAt, sr.IsReleased })
            .HasDatabaseName("IX_StockReservations_ExpiresAt_IsReleased");
    }
}
