using AppCore.Domains.Common;
using AppCore.Enums;

namespace AppCore.Domains.Pricing;

public class Price : AuditableEntity
{
    public long ProductVariantId { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public PriceType PriceType { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int? MinQuantity { get; set; }
    public Product.ProductVariant ProductVariant { get; set; } = null!;
    public Currency Currency { get; set; } = null!;
}
