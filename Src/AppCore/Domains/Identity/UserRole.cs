using AppCore.Domains.Common;

namespace AppCore.Domains.Identity;

public class UserRole : BaseEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
