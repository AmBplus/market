using AppCore.Domains.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Shop;

public class GuaranteeConfiguration : IEntityTypeConfiguration<Guarantee>
{
    public void Configure(EntityTypeBuilder<Guarantee> builder)
    {
        builder.ToTable("Guarantees", "Shop");
        builder.Property(g => g.Title).IsRequired().HasMaxLength(200);
    }
}
