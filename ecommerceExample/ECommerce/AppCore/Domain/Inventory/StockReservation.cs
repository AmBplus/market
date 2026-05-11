using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Inventory;

public class StockReservation : BaseEntity
{
    public long ProductVariantId { get; set; }
    public long WarehouseId { get; set; }
    public long OrderId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public bool IsReleased { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public Product.ProductVariant ProductVariant { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}
