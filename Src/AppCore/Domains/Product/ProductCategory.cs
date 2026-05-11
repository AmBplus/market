using AppCore.Domains.Common;

namespace AppCore.Domains.Product;

public class ProductCategory : BaseEntity
{
    public long ProductId { get; set; }
    public long CategoryId { get; set; }
    public int DisplayOrder { get; set; }
    public Product Product { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
