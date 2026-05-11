using AppCore.Domains.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Inventory;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses", "Inventory");

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.Address)
            .HasMaxLength(500);

        builder.Property(w => w.City)
            .HasMaxLength(100);

        builder.Property(w => w.Province)
            .HasMaxLength(100);

        builder.Property(w => w.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(w => w.Code)
            .IsUnique()
            .HasDatabaseName("IX_Warehouses_Code");
    }
}
