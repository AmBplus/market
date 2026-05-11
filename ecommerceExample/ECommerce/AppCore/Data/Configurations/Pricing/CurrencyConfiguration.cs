using ECommerce.AppCore.Domain.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Pricing;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies", "Pricing");

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.Name)
            .HasMaxLength(100);

        builder.Property(c => c.Symbol)
            .HasMaxLength(10);

        builder.Property(c => c.ExchangeRate)
            .HasPrecision(18, 6);

        builder.Property(c => c.IsBase)
            .HasDefaultValue(false);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Currencies_Code");
    }
}
