using ECommerce.AppCore.Domain.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Shipping;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.ToTable("DeliveryMethods", "Shipping");

        builder.Property(dm => dm.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(dm => dm.Price)
            .HasPrecision(18, 2);

        builder.Property(dm => dm.Description)
            .HasMaxLength(500);

        builder.Property(dm => dm.ConstraintsJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(dm => dm.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(dm => dm.DeliveryType)
            .HasDatabaseName("IX_DeliveryMethods_DeliveryType");

        builder.HasIndex(dm => dm.IsActive)
            .HasDatabaseName("IX_DeliveryMethods_IsActive");
    }
}
