using AppCore.Domains.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Order;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", "Order");

        builder.Property(ci => ci.UnitPrice)
            .HasPrecision(18, 2);

        builder.HasIndex(ci => new { ci.CartId, ci.ProductVariantId })
            .IsUnique()
            .HasDatabaseName("IX_CartItems_CartId_ProductVariantId");
    }
}
