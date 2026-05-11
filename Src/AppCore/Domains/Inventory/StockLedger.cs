using AppCore.Domains.Common;
using AppCore.Enums;

namespace AppCore.Domains.Inventory;

public class StockLedger : BaseEntity
{
    public long ProductVariantId { get; set; }
    public long WarehouseId { get; set; }
    public int QuantityChange { get; set; }
    public StockChangeReason Reason { get; set; }
    public long? ReferenceId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public long? CreatedBy { get; set; }
    public Product.ProductVariant ProductVariant { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}
