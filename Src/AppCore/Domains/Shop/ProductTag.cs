using AppCore.Domains.Common;

namespace AppCore.Domains.Shop;

public class ProductTag : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
