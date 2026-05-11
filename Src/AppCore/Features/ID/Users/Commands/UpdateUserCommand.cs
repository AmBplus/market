using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;



namespace AppCore.Features.ID.Users.Commands;
    // AppCore/Features/ID/Users/Commands/UpdateUser/UpdateUserCommand.cs

    public class UpdateUserCommand
{
    public long Id { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FatherName { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public long? UpdatedBy { get; set; }
}

public class UpdateUserHandler
{
    private readonly AppDbContext _context;

    public UpdateUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation> Handle(UpdateUserCommand command)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == command.Id && !u.IsDelete);

        if (user is null)
            return ResultOperation.ToFailedResult("کاربر مورد نظر یافت نشد.");

        // بررسی تکراری نبودن username (به جز خودش)
        var userNameExists = await _context.Users
            .AnyAsync(u => u.UserName == command.UserName
                        && u.Id != command.Id
                        && !u.IsDelete);

        if (userNameExists)
            return ResultOperation.ToFailedResult("این نام کاربری قبلاً ثبت شده است.");

        // بررسی تکراری نبودن موبایل (به جز خودش)
        if (!string.IsNullOrWhiteSpace(command.Mobile))
        {
            var mobileExists = await _context.Users
                .AnyAsync(u => u.Mobile == command.Mobile
                            && u.Id != command.Id
                            && !u.IsDelete);

            if (mobileExists)
                return ResultOperation.ToFailedResult("این شماره موبایل قبلاً ثبت شده است.");
        }

        // بررسی تکراری نبودن ایمیل (به جز خودش)
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == command.Email
                            && u.Id != command.Id
                            && !u.IsDelete);

            if (emailExists)
                return ResultOperation.ToFailedResult("این ایمیل قبلاً ثبت شده است.");
        }

        user.UserName = command.UserName;
        user.Email = command.Email;
        user.Mobile = command.Mobile;
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.FatherName = command.FatherName;
        user.NationalId = command.NationalId;
        user.PassportNumber = command.PassportNumber;
        user.Nationality = command.Nationality;
        user.BirthDate = command.BirthDate;
        user.Address = command.Address;
        user.IsActive = command.IsActive;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = command.UpdatedBy;

        await _context.SaveChangesAsync();

        return ResultOperation.ToSuccessResult("کاربر با موفقیت ویرایش شد.");
    }
}
