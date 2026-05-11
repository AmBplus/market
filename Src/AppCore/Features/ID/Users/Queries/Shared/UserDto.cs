

namespace AppCore.Features.ID.Users.Queries.Shared;


public class UserDto
{
    public long Id { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string PasswordHash { get; set; }
    public string? NationalCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }
    public bool IsDelete { get; set; }


    public string? FatherName { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }

    public List<RoleDomainDto>? RolesDomain { get; set; }

    public ProfileDto? Profile { get; set; }
}
