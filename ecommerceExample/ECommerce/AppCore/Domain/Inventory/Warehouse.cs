using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Inventory;

public class Warehouse : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<StockLedger> StockLedgers { get; set; } = new List<StockLedger>();
    public ICollection<StockReservation> StockReservations { get; set; } = new List<StockReservation>();
}
