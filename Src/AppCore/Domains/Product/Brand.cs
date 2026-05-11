using AppCore.Domains.Common;

namespace AppCore.Domains.Product;

public class Brand : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }
    public string? WebsiteUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
