using AppCore.Domains.Common;

namespace AppCore.Domains.Discount;

public class ProductDiscount : BaseEntity
{
    public long DiscountId { get; set; }
    public long ProductId { get; set; }
    public long? ProductVariantId { get; set; }
    public Discount Discount { get; set; } = null!;
    public Product.Product Product { get; set; } = null!;
    public Product.ProductVariant? ProductVariant { get; set; }
}
