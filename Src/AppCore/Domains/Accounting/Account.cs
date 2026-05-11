using AppCore.Domains.Common;
using AppCore.Enums;

namespace AppCore.Domains.Accounting;

public class Account : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public long? ParentId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public Account? Parent { get; set; }
    public ICollection<Account> Children { get; set; } = new List<Account>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
