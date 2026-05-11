using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Product;

public class ProductImage : BaseEntity
{
    public long ProductId { get; set; }
    public long? ProductVariantId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public Product Product { get; set; } = null!;
    public ProductVariant? ProductVariant { get; set; }
}
