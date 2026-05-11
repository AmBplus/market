using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Enums;

namespace ECommerce.AppCore.Domain.Order;

public class OrderStatusHistory : BaseEntity
{
    public long OrderId { get; set; }
    public OrderStatus FromStatus { get; set; }
    public OrderStatus ToStatus { get; set; }
    public string? Note { get; set; }
    public long? ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public Order Order { get; set; } = null!;
}
