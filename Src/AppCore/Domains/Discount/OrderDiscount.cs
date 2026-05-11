using AppCore.Domains.Common;

namespace AppCore.Domains.Discount;

public class OrderDiscount : BaseEntity
{
    public long OrderId { get; set; }
    public long DiscountId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public Order.Order Order { get; set; } = null!;
    public Discount Discount { get; set; } = null!;
}
