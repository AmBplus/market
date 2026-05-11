using AppCore.Domains.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Shop;

public class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("Colors", "Shop");
        builder.Property(c => c.Title).IsRequired().HasMaxLength(100);
        builder.Property(c => c.ColorCode).IsRequired().HasMaxLength(20);
        builder.HasIndex(c => c.ColorCode).IsUnique();
    }
}
