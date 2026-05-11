

namespace AppCore.Features.ID.Users.Queries.Shared;

public class RoleDomainDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public required string RoleName { get; set; }
}
