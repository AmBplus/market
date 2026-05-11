using AppCore.Domains.Common;

namespace AppCore.Domains.Product;

public class ProductAttribute : BaseEntity
{
    public long ProductId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Group { get; set; }
    public int DisplayOrder { get; set; }
    public Product Product { get; set; } = null!;
}
