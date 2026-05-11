using AppCore.Domains.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Configurations.Payment;

public class PaymentGatewayConfiguration : IEntityTypeConfiguration<PaymentGateway>
{
    public void Configure(EntityTypeBuilder<PaymentGateway> builder)
    {
        builder.ToTable("PaymentGateways", "Payment");

        builder.Property(pg => pg.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pg => pg.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pg => pg.ConfigJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(pg => pg.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(pg => pg.Code)
            .IsUnique()
            .HasDatabaseName("IX_PaymentGateways_Code");
    }
}
