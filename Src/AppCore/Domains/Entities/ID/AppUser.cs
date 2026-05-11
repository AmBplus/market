using AppCore.Domains.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
namespace AppCore.Domains.Entities.ID;
public class AppUser : BaseEntity
{
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? Mobile { get; set; }
    public bool IsConfirmedMobile { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDelete { get; set; } = false;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FatherName { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public ICollection<AppUserDomainRole> DomainRoles { get; set; } = new List<AppUserDomainRole>();
    public ICollection<UserRolePermission> Permissions { get; set; } = new List<UserRolePermission>();
}
