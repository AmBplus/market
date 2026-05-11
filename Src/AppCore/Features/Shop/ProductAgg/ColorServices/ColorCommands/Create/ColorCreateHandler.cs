using AppCore.Data;
using AppCore.Domains.Shop;
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
        var titleExists = await _context.Colors
            .AnyAsync(c => c.Title == command.Title && !c.IsDeleted);

        if (titleExists)
            return ResultOperation<long>.ToFailedResult("این عنوان قبلاً ثبت شده است.");

        var colorCodeExists = await _context.Colors
            .AnyAsync(c => c.ColorCode == command.ColorCode && !c.IsDeleted);

        if (colorCodeExists)
            return ResultOperation<long>.ToFailedResult("این کد رنگ قبلاً ثبت شده است.");

        var color = new Color
        {
            Title = command.Title,
            ColorCode = command.ColorCode,
            IsDeleted = false,
            IsActive = true,
            CreatedBy = command.CreateBy
        };

        await _context.Colors.AddAsync(color);
        await _context.SaveChangesAsync();

        return ResultOperation<long>.ToSuccessResult("رنگ با موفقیت ایجاد شد.", color.Id);
    }
}
