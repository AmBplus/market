using AppCore.Domains.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Inventory;

public class StockLedgerConfiguration : IEntityTypeConfiguration<StockLedger>
{
    public void Configure(EntityTypeBuilder<StockLedger> builder)
    {
        builder.ToTable("StockLedgers", "Inventory");

        builder.Property(sl => sl.Description)
            .HasMaxLength(500);

        // Critical composite index: ProductVariantId + WarehouseId for SUM calculation
        builder.HasIndex(sl => new { sl.ProductVariantId, sl.WarehouseId })
            .HasDatabaseName("IX_StockLedgers_ProductVariantId_WarehouseId");

        builder.HasIndex(sl => sl.CreatedAt)
            .HasDatabaseName("IX_StockLedgers_CreatedAt");

        builder.HasIndex(sl => sl.Reason)
            .HasDatabaseName("IX_StockLedgers_Reason");

        builder.HasIndex(sl => sl.ReferenceId)
            .HasDatabaseName("IX_StockLedgers_ReferenceId");

        builder.HasIndex(sl => new { sl.ProductVariantId, sl.CreatedAt })
            .HasDatabaseName("IX_StockLedgers_ProductVariantId_CreatedAt");
    }
}
