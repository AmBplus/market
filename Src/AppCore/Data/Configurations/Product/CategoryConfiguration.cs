using AppCore.Domains.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Product;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "Product");

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(2000);

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(500);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Code");

        builder.HasIndex(c => c.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Slug");

        builder.HasIndex(c => c.ParentId)
            .HasDatabaseName("IX_Categories_ParentId");

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
