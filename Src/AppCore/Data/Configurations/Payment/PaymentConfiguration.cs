using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Payment;

public class PaymentConfiguration : IEntityTypeConfiguration<Domains.Payment.Payment>
{
    public void Configure(EntityTypeBuilder<Domains.Payment.Payment> builder)
    {
        builder.ToTable("Payments", "Payment");
        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.TransactionRef).HasMaxLength(100);
        builder.Property(p => p.TrackingCode).HasMaxLength(100);
        builder.Property(p => p.PaymentToken).HasMaxLength(500);
        builder.Property(p => p.CallbackUrl).HasMaxLength(500);
        builder.Property(p => p.BankMessage).HasMaxLength(500);
        builder.HasIndex(p => p.TransactionRef).IsUnique().HasFilter("[TransactionRef] IS NOT NULL").HasDatabaseName("IX_Payments_TransactionRef");
        builder.HasIndex(p => p.OrderId).HasDatabaseName("IX_Payments_OrderId");
        builder.HasIndex(p => p.GatewayId).HasDatabaseName("IX_Payments_GatewayId");
        builder.HasIndex(p => p.Status).HasDatabaseName("IX_Payments_Status");
        builder.HasIndex(p => new { p.OrderId, p.Status }).HasDatabaseName("IX_Payments_OrderId_Status");
    }
}
