using AppCore.Domains.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Identity;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles", "Identity");

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique()
            .HasDatabaseName("IX_UserRoles_UserId_RoleId");

        builder.HasIndex(ur => ur.RoleId)
            .HasDatabaseName("IX_UserRoles_RoleId");
    }
}
