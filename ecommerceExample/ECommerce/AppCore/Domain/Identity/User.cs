using ECommerce.AppCore.Domain.Common;
using ECommerce.AppCore.Domain.Order;

namespace ECommerce.AppCore.Domain.Identity;

public class User : AuditableEntity
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public string? DefaultAddressesJson { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpireAt { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Order.Order> Orders { get; set; } = new List<Order.Order>();
    public ICollection<Order.Cart> Carts { get; set; } = new List<Order.Cart>();
}
