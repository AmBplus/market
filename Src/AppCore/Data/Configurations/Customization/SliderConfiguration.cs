using AppCore.Domains.Customization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Customization;

public class SliderConfiguration : IEntityTypeConfiguration<Slider>
{
    public void Configure(EntityTypeBuilder<Slider> builder)
    {
        builder.ToTable("Sliders", "Customization");

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.SubTitle)
            .HasMaxLength(300);

        builder.Property(s => s.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.MobileImageUrl)
            .HasMaxLength(500);

        builder.Property(s => s.LinkUrl)
            .HasMaxLength(500);

        builder.Property(s => s.LinkText)
            .HasMaxLength(100);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(s => new { s.IsActive, s.DisplayOrder })
            .HasDatabaseName("IX_Sliders_IsActive_DisplayOrder");
    }
}
