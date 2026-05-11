using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Pricing;

public class Currency : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; } = 1.0m;
    public bool IsBase { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Price> Prices { get; set; } = new List<Price>();
}
