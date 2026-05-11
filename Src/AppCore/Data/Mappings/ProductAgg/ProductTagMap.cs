using AppCore.Domains.Entities.Shop.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ProductAgg
{
    public class ProductTagMap : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.ToTable("ProductTags", "Shop");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.HasIndex(x => x.Slug).IsUnique();
        }
    }
}
