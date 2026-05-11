using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.Commands;


public record ColorCreateCommand(
    string Title,
    string ColorCode,
    long? CreateBy
);

public class ColorCreateHandler
{
    private readonly AppDbContext _context;

    public ColorCreateHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<long>> Handle(ColorCreateCommand command)
    {
        // بررسی تکراری نبودن Title
        var titleExists = await _context.Colors
            .AnyAsync(c => c.Title == command.Title && !c.IsDelete);

        if (titleExists)
            return ResultOperation<long>.ToFailedResult("این عنوان قبلاً ثبت شده است.");

        // بررسی تکراری نبودن ColorCode
        var colorCodeExists = await _context.Colors
            .AnyAsync(c => c.ColorCode == command.ColorCode && !c.IsDelete);

        if (colorCodeExists)
            return ResultOperation<long>.ToFailedResult("این کد رنگ قبلاً ثبت شده است.");

        // بررسی وجود کاربر ایجاد کننده
        if (command.CreateBy.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == command.CreateBy && !u.IsDelete);

            if (!userExists)
                return ResultOperation<long>.ToFailedResult("کاربر مورد نظر برای ایجاد یافت نشد.");
        }

        var color = new Color
        {
            Title = command.Title,
            ColorCode = command.ColorCode,
            CreateBy = command.CreateBy ?? 0,
            CreateAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDelete = false,
            IsActive = true
        };

        await _context.Colors.AddAsync(color);
        await _context.SaveChangesAsync();

        return ResultOperation<long>.ToSuccessResult("رنگ با موفقیت ایجاد شد.", color.Id);
    }
}
