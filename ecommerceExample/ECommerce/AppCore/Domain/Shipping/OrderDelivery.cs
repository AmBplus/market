using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Enums;

namespace ECommerce.AppCore.Domain.Shipping;

public class OrderDelivery : AuditableEntity
{
    public long OrderId { get; set; }
    public long DeliveryMethodId { get; set; }
    public DeliveryStatus Status { get; set; }
    public string? DeliveryAddressJson { get; set; }
    public string? TrackingCode { get; set; }
    public string? DigitalContent { get; set; }
    public DigitalContentType? DigitalContentType { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? Note { get; set; }
    public Order.Order Order { get; set; } = null!;
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
}
