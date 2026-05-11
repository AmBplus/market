using AppCore.Domains.Entities.ID;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings.ID
{
    public class AppUserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Email)
                .HasMaxLength(200);

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.UserName)
                .IsUnique();

            builder.HasIndex(x => x.Email);

            builder.Property(x => x.FirstName)
            .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasMaxLength(100);

            builder.Property(x => x.FatherName)
                .HasMaxLength(100);

            builder.Property(x => x.NationalId)
                .HasMaxLength(20);

            builder.Property(x => x.PassportNumber)
                .HasMaxLength(20);

            builder.Property(x => x.Nationality)
                .HasMaxLength(100);
            builder.HasMany(x => x.DomainRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
