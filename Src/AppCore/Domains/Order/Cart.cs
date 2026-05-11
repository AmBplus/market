using AppCore.Domains.Common;

namespace AppCore.Domains.Order;

public class Cart : AuditableEntity
{
    public long UserId { get; set; }
    public string? DiscountCode { get; set; }
    public bool IsActive { get; set; } = true;
    public Identity.User User { get; set; } = null!;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
