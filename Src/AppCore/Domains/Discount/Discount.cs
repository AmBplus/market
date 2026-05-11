using AppCore.Domains.Common;
using AppCore.Enums;

namespace AppCore.Domains.Discount;

public class Discount : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public DiscountScope Scope { get; set; }
    public decimal Value { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public int? PerUserLimit { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
    public ICollection<CategoryDiscount> CategoryDiscounts { get; set; } = new List<CategoryDiscount>();
    public ICollection<OrderDiscount> OrderDiscounts { get; set; } = new List<OrderDiscount>();
}
