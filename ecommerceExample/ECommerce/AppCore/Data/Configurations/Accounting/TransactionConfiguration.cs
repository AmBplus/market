using ECommerce.AppCore.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.AppCore.Data.Configurations.Accounting;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions", "Accounting");

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.HasIndex(t => t.AccountId)
            .HasDatabaseName("IX_Transactions_AccountId");

        builder.HasIndex(t => t.BatchId)
            .HasDatabaseName("IX_Transactions_BatchId");

        builder.HasIndex(t => t.CreatedAt)
            .HasDatabaseName("IX_Transactions_CreatedAt");

        builder.HasIndex(t => t.Source)
            .HasDatabaseName("IX_Transactions_Source");

        builder.HasIndex(t => new { t.AccountId, t.CreatedAt })
            .HasDatabaseName("IX_Transactions_AccountId_CreatedAt");
    }
}
