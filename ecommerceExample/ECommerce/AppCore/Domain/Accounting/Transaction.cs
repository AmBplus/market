using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Enums;

namespace ECommerce.AppCore.Domain.Accounting;

public class Transaction : BaseEntity
{
    public long AccountId { get; set; }
    public Guid BatchId { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public TransactionSource Source { get; set; }
    public long? ReferenceId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public long? CreatedBy { get; set; }
    public Account Account { get; set; } = null!;
    public Pricing.Currency Currency { get; set; } = null!;
}
