using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Features.ID.Users.Commands;
using AppCore.Data;
using AppCore.Domains.Entities.ID;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;


public class CreateUserCommand
{
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }
    public string? Mobile { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FatherName { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public long? CreatedBy { get; set; }
}

public class CreateUserHandler
{
    private readonly AppDbContext _context;

    public CreateUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<long>> Handle(CreateUserCommand command)
    {
        // بررسی تکراری نبودن username
        var userNameExists = await _context.Users
            .AnyAsync(u => u.UserName == command.UserName && !u.IsDelete);

        if (userNameExists)
            return ResultOperation<long>.ToFailedResult("این نام کاربری قبلاً ثبت شده است.");

        // بررسی تکراری نبودن موبایل
        if (!string.IsNullOrWhiteSpace(command.Mobile))
        {
            var mobileExists = await _context.Users
                .AnyAsync(u => u.Mobile == command.Mobile && !u.IsDelete);

            if (mobileExists)
                return ResultOperation<long>.ToFailedResult("این شماره موبایل قبلاً ثبت شده است.");
        }

        // بررسی تکراری نبودن ایمیل
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == command.Email && !u.IsDelete);

            if (emailExists)
                return ResultOperation<long>.ToFailedResult("این ایمیل قبلاً ثبت شده است.");
        }

        var user = new AppUser
        {
            UserName = command.UserName,
            Email = command.Email,
            PasswordHash = command.Password, // Must Be Hashed
            Mobile = command.Mobile,
            FirstName = command.FirstName,
            LastName = command.LastName,
            FatherName = command.FatherName,
            NationalId = command.NationalId,
            PassportNumber = command.PassportNumber,
            Nationality = command.Nationality,
            BirthDate = command.BirthDate,
            Address = command.Address,
            IsActive = command.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = command.CreatedBy,
            UpdatedBy = command.CreatedBy
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return ResultOperation<long>.ToSuccessResult("کاربر با موفقیت ایجاد شد.", user.Id);
    }
}
