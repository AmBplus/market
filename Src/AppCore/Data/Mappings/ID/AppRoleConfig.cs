using AppCore.Domains.Entities.ID;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ID
{
    public class AppRoleMap : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
    .ValueGeneratedNever();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(x => x.Title)
                .IsUnique();

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
