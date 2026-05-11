using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Discount;

public class CategoryDiscount : BaseEntity
{
    public long DiscountId { get; set; }
    public long CategoryId { get; set; }
    public Discount Discount { get; set; } = null!;
    public Product.Category Category { get; set; } = null!;
}
