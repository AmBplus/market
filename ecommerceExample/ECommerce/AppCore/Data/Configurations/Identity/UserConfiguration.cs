using ECommerce.AppCore.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "Identity");

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .HasMaxLength(200);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.PasswordSalt)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.NationalCode)
            .HasMaxLength(10);

        builder.Property(u => u.DefaultAddressesJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(u => u.UserName)
            .IsUnique()
            .HasDatabaseName("IX_Users_UserName");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL")
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL")
            .HasDatabaseName("IX_Users_PhoneNumber");

        builder.HasIndex(u => u.NationalCode)
            .IsUnique()
            .HasFilter("[NationalCode] IS NOT NULL")
            .HasDatabaseName("IX_Users_NationalCode");

        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_Users_IsActive");

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
