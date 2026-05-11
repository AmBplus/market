using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Product;

public class ProductCategory : BaseEntity
{
    public long ProductId { get; set; }
    public long CategoryId { get; set; }
    public int DisplayOrder { get; set; }
    public Product Product { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
