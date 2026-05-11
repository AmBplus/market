using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Discount;

public class ProductDiscount : BaseEntity
{
    public long DiscountId { get; set; }
    public long ProductId { get; set; }
    public long? ProductVariantId { get; set; }
    public Discount Discount { get; set; } = null!;
    public Product.Product Product { get; set; } = null!;
    public Product.ProductVariant? ProductVariant { get; set; }
}
