using AppCore.Domains.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Identity;

public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder.ToTable("PermissionTypes", "Identity");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
    }
}
