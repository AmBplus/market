using AppCore.Domains.Customization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Customization;

public class WidgetConfiguration : IEntityTypeConfiguration<Widget>
{
    public void Configure(EntityTypeBuilder<Widget> builder)
    {
        builder.ToTable("Widgets", "Customization");

        builder.Property(w => w.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.ConfigJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(w => w.Position)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(w => new { w.Position, w.DisplayOrder })
            .HasDatabaseName("IX_Widgets_Position_DisplayOrder");
    }
}
