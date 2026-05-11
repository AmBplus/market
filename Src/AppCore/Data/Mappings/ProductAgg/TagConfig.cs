using AppCore.Domains.Entities.Shop.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppCore.Data.Mappings.ProductAgg;


public class TagConfig : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("Tags", "shop");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Slug)
            .HasMaxLength(150);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.HasIndex(t => t.Slug)
            .IsUnique()
            .HasFilter("[IsDelete] = 0");

        builder.HasIndex(t => t.Name)
            .HasFilter("[IsDelete] = 0");

        // فیلدهای base
        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();
    }
}

