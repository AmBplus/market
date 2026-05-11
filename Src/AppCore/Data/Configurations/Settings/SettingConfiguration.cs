using AppCore.Domains.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Settings;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Settings", "Settings");

        builder.Property(s => s.Key)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Value)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.Module)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.ValueType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.IsEditable)
            .HasDefaultValue(true);

        builder.HasIndex(s => new { s.Key, s.Module })
            .IsUnique()
            .HasDatabaseName("IX_Settings_Key_Module");
    }
}
