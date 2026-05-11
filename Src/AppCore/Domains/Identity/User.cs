using AppCore.Domains.Common;
using AppCore.Domains.Order;

namespace AppCore.Domains.Identity;

public class User : SoftDeletableEntity
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? FatherName { get; set; }
    public string? NationalCode { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Avatar { get; set; }
    public string? DefaultAddressesJson { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpireAt { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Order.Order> Orders { get; set; } = new List<Order.Order>();
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public ICollection<UserRolePermission> Permissions { get; set; } = new List<UserRolePermission>();
}
