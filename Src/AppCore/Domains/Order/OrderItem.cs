using AppCore.Domains.Common;

namespace AppCore.Domains.Order;

public class OrderItem : BaseEntity
{
    public long OrderId { get; set; }
    public long ProductVariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string VariantTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public long? VendorId { get; set; }
    public decimal? VendorCommissionRate { get; set; }
    public Order Order { get; set; } = null!;
    public Product.ProductVariant ProductVariant { get; set; } = null!;
    public Vendor.Vendor? Vendor { get; set; }
}
