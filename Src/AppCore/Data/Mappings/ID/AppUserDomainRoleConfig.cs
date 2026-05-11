using AppCore.Domains.Entities.ID;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ID
{
    public class AppUserDomainRoleMap : IEntityTypeConfiguration<AppUserDomainRole>
    {
        public void Configure(EntityTypeBuilder<AppUserDomainRole> builder)
        {
            builder.ToTable("AppUserDomainRoles");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.DomainRoles)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);

            builder.HasIndex(x => new { x.UserId, x.RoleId })
                .IsUnique();
        }
    }
}
