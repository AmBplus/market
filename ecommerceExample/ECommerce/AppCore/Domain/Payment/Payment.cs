using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Enums;

namespace ECommerce.AppCore.Domain.Payment;

public class Payment : AuditableEntity
{
    public long OrderId { get; set; }
    public long GatewayId { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionRef { get; set; }
    public string? TrackingCode { get; set; }
    public string? PaymentToken { get; set; }
    public string? CallbackUrl { get; set; }
    public string? BankMessage { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Order.Order Order { get; set; } = null!;
    public PaymentGateway Gateway { get; set; } = null!;
    public Pricing.Currency Currency { get; set; } = null!;
}
