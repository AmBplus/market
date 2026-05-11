using AppCore.Domains.Entities.Shop.ProductAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ProductAgg
{
    public class ProductBrandMap : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.ToTable("ProductBrands", "Shop");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.LogoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.WebsiteUrl)
                .HasMaxLength(256);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.HasIndex(x => x.Slug).IsUnique();
        }
    }
}
