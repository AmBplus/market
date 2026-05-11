using AppCore.Domains.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Settings;

public class EnumValueConfiguration : IEntityTypeConfiguration<EnumValue>
{
    public void Configure(EntityTypeBuilder<EnumValue> builder)
    {
        builder.ToTable("EnumValues", "Settings");

        builder.Property(ev => ev.EnumType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ev => ev.EnumKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ev => ev.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ev => ev.Description)
            .HasMaxLength(500);

        builder.Property(ev => ev.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(ev => new { ev.EnumType, ev.EnumKey })
            .IsUnique()
            .HasDatabaseName("IX_EnumValues_EnumType_EnumKey");

        builder.HasIndex(ev => ev.EnumType)
            .HasDatabaseName("IX_EnumValues_EnumType");

        builder.HasIndex(ev => new { ev.EnumType, ev.EnumId })
            .HasDatabaseName("IX_EnumValues_EnumType_EnumId");
    }
}
