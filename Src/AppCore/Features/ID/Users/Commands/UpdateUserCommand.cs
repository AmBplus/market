using AppCore.Data;
using AppCore.Domains.Identity;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Users.Commands;

public class UpdateUserCommand
{
    public long Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateUserHandler
{
    private readonly AppDbContext _context;
    public UpdateUserHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateUserCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.FirstName))
            return ResultOperation.ToFailedResult("نام الزامی است");

        if (string.IsNullOrWhiteSpace(command.LastName))
            return ResultOperation.ToFailedResult("نام خانوادگی الزامی است");

        var user = await _context.Users.FindAsync(new object[] { command.Id }, ct);
        if (user is null)
            return ResultOperation.ToFailedResult("کاربر یافت نشد");

        if (!string.IsNullOrWhiteSpace(command.Email) && command.Email != user.Email)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == command.Email && u.Id != command.Id, ct);
            if (emailExists)
                return ResultOperation.ToFailedResult("ایمیل تکراری است");
        }

        user.Email = command.Email;
        user.PhoneNumber = command.PhoneNumber;
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.NationalCode = command.NationalCode;
        user.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("کاربر با موفقیت ویرایش شد");
    }
}
