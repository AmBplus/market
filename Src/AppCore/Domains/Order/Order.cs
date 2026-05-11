using AppCore.Domains.Common;
using AppCore.Enums;

namespace AppCore.Domains.Order;

public class Order : AuditableEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public long UserId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public long CurrencyId { get; set; }
    public decimal CurrencyRate { get; set; } = 1.0m;
    public string? CustomerNote { get; set; }
    public string? AdminNote { get; set; }
    public string ShippingAddressJson { get; set; } = string.Empty;
    public DateTime? CompletedAt { get; set; }
    public Identity.User User { get; set; } = null!;
    public Pricing.Currency Currency { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
    public ICollection<Payment.Payment> Payments { get; set; } = new List<Payment.Payment>();
    public ICollection<Shipping.OrderDelivery> Deliveries { get; set; } = new List<Shipping.OrderDelivery>();
    public ICollection<Discount.OrderDiscount> OrderDiscounts { get; set; } = new List<Discount.OrderDiscount>();
    public ICollection<Inventory.StockReservation> StockReservations { get; set; } = new List<Inventory.StockReservation>();
}
