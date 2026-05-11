using AppCore.Domains.Entities.ID;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ID
{
    public class UserRolePermissionMap : IEntityTypeConfiguration<UserRolePermission>
    {
        public void Configure(EntityTypeBuilder<UserRolePermission> builder)
        {
            builder.ToTable("UserRolePermissions", t =>
            {
                t.HasCheckConstraint(
                    "CK_UserRolePermission_UserOrRole",
                    "[UserId] IS NOT NULL OR [RoleId] IS NOT NULL"
                );
            });

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsAllowed)
                .IsRequired();

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.PermissionId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
