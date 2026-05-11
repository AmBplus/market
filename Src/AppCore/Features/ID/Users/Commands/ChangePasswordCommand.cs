using System.Security.Cryptography;
using System.Text;
using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Users.Commands;

public class ChangePasswordCommand
{
    public long UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ChangePasswordHandler
{
    private readonly AppDbContext _context;
    public ChangePasswordHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(ChangePasswordCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.CurrentPassword))
            return ResultOperation.ToFailedResult("رمز عبور فعلی الزامی است");

        if (string.IsNullOrWhiteSpace(command.NewPassword))
            return ResultOperation.ToFailedResult("رمز عبور جدید الزامی است");

        var user = await _context.Users.FindAsync(new object[] { command.UserId }, ct);
        if (user is null)
            return ResultOperation.ToFailedResult("کاربر یافت نشد");

        if (!VerifyPassword(command.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            return ResultOperation.ToFailedResult("رمز عبور فعلی نادرست است");

        var (hash, salt) = HashPassword(command.NewPassword);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("رمز عبور با موفقیت تغییر یافت");
    }

    private static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + storedSalt));
        var hash = Convert.ToBase64String(hashBytes);
        return hash == storedHash;
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
