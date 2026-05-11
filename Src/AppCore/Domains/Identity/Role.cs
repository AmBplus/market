using AppCore.Domains.Common;

namespace AppCore.Domains.Identity;

public class Role : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserRolePermission> Permissions { get; set; } = new List<UserRolePermission>();
}
