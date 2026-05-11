using AppCore.Domains.Entities.Shop.ProductAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ProductAgg
{
    public class ColorMap : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Colors", "Shop");

            builder.HasKey(x => x.Id);

            // Title - max 300 characters
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            // ColorCode - max 300 characters (unique to prevent duplicate colors)
            builder.Property(x => x.ColorCode)
                .IsRequired()
                .HasMaxLength(300);

            // CreateBy lifecycle field
            builder.Property(x => x.CreateBy);

            // CreateAt lifecycle field
            builder.Property(x => x.CreateAt);

            // UpdateBy lifecycle field
            builder.Property(x => x.UpdateBy);

            // UpdateAt lifecycle field
            builder.Property(x => x.UpdateAt);

            // IsUpdate flag
            builder.Property(x => x.IsUpdate)
                .IsRequired();

            // IsDelete - soft delete flag (consistent with AppUser, ProductBrand patterns)
            builder.Property(x => x.IsDelete)
                .IsRequired();

            // IsActive - active status flag
            builder.Property(x => x.IsActive)
                .IsRequired();

            // Unique index on ColorCode to prevent duplicate colors
            builder.HasIndex(x => x.ColorCode).IsUnique();
        }
    }
}
