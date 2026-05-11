using AppCore.Domains.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Identity;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions", "Identity");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.HasOne(x => x.PermissionType)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.PermissionTypeId);
    }
}
