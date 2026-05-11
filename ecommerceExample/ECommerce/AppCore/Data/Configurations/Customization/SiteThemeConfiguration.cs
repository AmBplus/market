using ECommerce.AppCore.Domain.Customization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Customization;

public class SiteThemeConfiguration : IEntityTypeConfiguration<SiteTheme>
{
    public void Configure(EntityTypeBuilder<SiteTheme> builder)
    {
        builder.ToTable("SiteThemes", "Customization");

        builder.Property(st => st.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(st => st.PrimaryColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(st => st.SecondaryColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(st => st.BackgroundColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(st => st.TextColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(st => st.AccentColor)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(st => st.FontFamily)
            .HasMaxLength(100);

        builder.Property(st => st.ExtendedSettingsJson)
            .HasColumnType("nvarchar(max)");
    }
}
