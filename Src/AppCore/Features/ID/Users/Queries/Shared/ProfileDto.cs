

namespace AppCore.Features.ID.Users.Queries.Shared;

public class ProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FatherName { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    
}