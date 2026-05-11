using AppCore.Domains.Common;

namespace AppCore.Domains.Identity;

public class UserRolePermission : BaseEntity
{
    public long PermissionId { get; set; }
    public Permission? Permission { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    public long? RoleId { get; set; }
    public Role? Role { get; set; }
    public bool IsAllowed { get; set; } = true;
}
