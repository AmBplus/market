using AppCore.Domains.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Pricing;

public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ToTable("Prices", "Pricing");

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        // Critical composite index: ProductVariantId + PriceType + EndedAt (filtered NULL) for finding active price
        builder.HasIndex(p => new { p.ProductVariantId, p.PriceType })
            .HasFilter("[EndedAt] IS NULL")
            .HasDatabaseName("IX_Prices_ProductVariantId_PriceType_EndedAt_Active");

        builder.HasIndex(p => p.CurrencyId)
            .HasDatabaseName("IX_Prices_CurrencyId");

        builder.HasIndex(p => new { p.StartedAt, p.EndedAt })
            .HasDatabaseName("IX_Prices_StartedAt_EndedAt");
    }
}
