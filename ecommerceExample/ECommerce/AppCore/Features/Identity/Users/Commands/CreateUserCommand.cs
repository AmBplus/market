using System.Security.Cryptography;
using System.Text;
using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Identity;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Identity.Users.Commands;

public class CreateUserCommand
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public List<long> RoleIds { get; set; } = new();
}

public class CreateUserHandler
{
    private readonly ECommerceDbContext _context;
    public CreateUserHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateUserCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.UserName))
            return ResultOperation.ToFailedResult("نام کاربری الزامی است");

        if (string.IsNullOrWhiteSpace(command.Password))
            return ResultOperation.ToFailedResult("رمز عبور الزامی است");

        if (string.IsNullOrWhiteSpace(command.FirstName))
            return ResultOperation.ToFailedResult("نام الزامی است");

        if (string.IsNullOrWhiteSpace(command.LastName))
            return ResultOperation.ToFailedResult("نام خانوادگی الزامی است");

        var exists = await _context.Users.AnyAsync(u => u.UserName == command.UserName, ct);
        if (exists)
            return ResultOperation.ToFailedResult("نام کاربری تکراری است");

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == command.Email, ct);
            if (emailExists)
                return ResultOperation.ToFailedResult("ایمیل تکراری است");
        }

        var (hash, salt) = HashPassword(command.Password);

        var user = new User
        {
            UserName = command.UserName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = hash,
            PasswordSalt = salt,
            NationalCode = command.NationalCode,
            IsActive = true
        };

        _context.Add(user);
        await _context.SaveChangesAsync(ct);

        if (command.RoleIds is { Count: > 0 })
        {
            var validRoles = await _context.Roles
                .Where(r => command.RoleIds.Contains(r.Id) && r.IsActive)
                .Select(r => r.Id)
                .ToListAsync(ct);

            foreach (var roleId in validRoles)
            {
                _context.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId,
                    AssignedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync(ct);
        }

        return ResultOperation.ToSuccessResult("کاربر با موفقیت ایجاد شد");
    }

    private static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        var hash = Convert.ToBase64String(hashBytes);

        return (hash, salt);
    }
}
