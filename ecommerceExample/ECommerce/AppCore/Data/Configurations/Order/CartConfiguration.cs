using ECommerce.AppCore.Domain.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Order;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts", "Order");

        builder.Property(c => c.DiscountCode)
            .HasMaxLength(50);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(c => new { c.UserId, c.IsActive })
            .HasDatabaseName("IX_Carts_UserId_IsActive");
    }
}
