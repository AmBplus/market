using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Order;

public class CartItem : BaseEntity
{
    public long CartId { get; set; }
    public long ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public long CurrencyId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public Cart Cart { get; set; } = null!;
    public Product.ProductVariant ProductVariant { get; set; } = null!;
    public Pricing.Currency Currency { get; set; } = null!;
}
