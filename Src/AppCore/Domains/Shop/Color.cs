using AppCore.Domains.Common;

namespace AppCore.Domains.Shop;

public class Color : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
