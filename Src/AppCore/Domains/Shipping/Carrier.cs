using AppCore.Domains.Common;

namespace AppCore.Domains.Shipping;

public class Carrier : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? TrackingUrlTemplate { get; set; }
    public string? ConfigJson { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<DeliveryMethod> DeliveryMethods { get; set; } = new List<DeliveryMethod>();
}
