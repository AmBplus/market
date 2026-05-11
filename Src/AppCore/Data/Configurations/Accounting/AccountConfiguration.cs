using AppCore.Domains.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Accounting;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts", "Accounting");

        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasMaxLength(500);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(a => a.Code)
            .IsUnique()
            .HasDatabaseName("IX_Accounts_Code");

        builder.HasIndex(a => a.AccountType)
            .HasDatabaseName("IX_Accounts_AccountType");

        builder.HasIndex(a => a.ParentId)
            .HasDatabaseName("IX_Accounts_ParentId");

        builder.HasOne(a => a.Parent)
            .WithMany(a => a.Children)
            .HasForeignKey(a => a.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
