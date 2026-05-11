using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Product;

public class Brand : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
