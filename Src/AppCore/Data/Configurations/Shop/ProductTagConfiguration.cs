using AppCore.Domains.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Shop;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("ProductTags", "Shop");
        builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Slug).HasMaxLength(150);
        builder.HasIndex(t => t.Slug).HasFilter("[Slug] IS NOT NULL");
    }
}
