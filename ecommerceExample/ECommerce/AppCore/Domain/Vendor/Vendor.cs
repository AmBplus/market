using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Enums;

namespace ECommerce.AppCore.Domain.Vendor;

public class Vendor : AuditableEntity
{
    public long UserId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreSlug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public decimal CommissionRate { get; set; }
    public VendorStatus Status { get; set; }
    public string? BankAccountNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public Identity.User User { get; set; } = null!;
    public ICollection<Product.Product> Products { get; set; } = new List<Product.Product>();
    public ICollection<Product.ProductVariant> ProductVariants { get; set; } = new List<Product.ProductVariant>();
}
