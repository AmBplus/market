using AppCore.Domains.Common;
using AppCore.Enums;

namespace AppCore.Domains.Shipping;

public class DeliveryMethod : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public DeliveryType DeliveryType { get; set; }
    public long? CarrierId { get; set; }
    public decimal Price { get; set; }
    public long CurrencyId { get; set; }
    public int? EstimatedDays { get; set; }
    public string? Description { get; set; }
    public string? ConstraintsJson { get; set; }
    public DigitalContentType? DigitalContentType { get; set; }
    public bool IsActive { get; set; } = true;
    public Carrier? Carrier { get; set; }
    public Pricing.Currency Currency { get; set; } = null!;
    public ICollection<OrderDelivery> OrderDeliveries { get; set; } = new List<OrderDelivery>();
}
