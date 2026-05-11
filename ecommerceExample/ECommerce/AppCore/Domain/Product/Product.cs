using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Domain.Discount;

namespace ECommerce.AppCore.Domain.Product;

public class Product : SoftDeletableEntity
{
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public long? BrandId { get; set; }
    public long? VendorId { get; set; }
    public string? ShippingConstraintsJson { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public Brand? Brand { get; set; }
    public Vendor.Vendor? Vendor { get; set; }
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
}
