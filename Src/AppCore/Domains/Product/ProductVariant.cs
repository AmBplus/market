using AppCore.Domains.Common;
using AppCore.Domains.Pricing;
using AppCore.Domains.Inventory;
using AppCore.Domains.Order;
using AppCore.Domains.Discount;

namespace AppCore.Domains.Product;

public class ProductVariant : SoftDeletableEntity
{
    public long ProductId { get; set; }
    public string VariantCode { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? VariantAttributesJson { get; set; }
    public long? VendorId { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal DefaultPrice { get; set; }
    public long CurrencyId { get; set; }
    public decimal? WeightGrams { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; }
    public Product Product { get; set; } = null!;
    public Vendor.Vendor? Vendor { get; set; }
    public Currency Currency { get; set; } = null!;
    public ICollection<Price> Prices { get; set; } = new List<Price>();
    public ICollection<StockLedger> StockLedgers { get; set; } = new List<StockLedger>();
    public ICollection<StockReservation> StockReservations { get; set; } = new List<StockReservation>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
}
