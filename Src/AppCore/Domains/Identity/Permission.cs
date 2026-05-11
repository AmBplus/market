using AppCore.Domains.Common;

namespace AppCore.Domains.Identity;

public class Permission : AuditableEntity
{
    public required string Title { get; set; }
    public long PermissionTypeId { get; set; }
    public PermissionType? PermissionType { get; set; }
    public ICollection<UserRolePermission>? Assignments { get; set; }
}
