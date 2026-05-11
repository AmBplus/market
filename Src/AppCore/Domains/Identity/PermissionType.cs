using AppCore.Domains.Common;

namespace AppCore.Domains.Identity;

public class PermissionType : AuditableEntity
{
    public required string Title { get; set; }
    public ICollection<Permission>? Permissions { get; set; }
}
