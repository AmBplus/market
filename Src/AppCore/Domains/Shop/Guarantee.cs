using AppCore.Domains.Common;

namespace AppCore.Domains.Shop;

public class Guarantee : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
