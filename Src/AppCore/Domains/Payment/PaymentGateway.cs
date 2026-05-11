using AppCore.Domains.Common;

namespace AppCore.Domains.Payment;

public class PaymentGateway : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
