using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.ColorCommands.Delete;

public record ColorDeleteCommand(long Id, long? DeletedBy);

public class ColorDeleteHandler
{
    private readonly AppDbContext _context;

    public ColorDeleteHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation> Handle(ColorDeleteCommand command)
    {
        var color = await _context.Colors
            .FirstOrDefaultAsync(c => c.Id == command.Id && !c.IsDeleted);

        if (color is null)
            return ResultOperation.ToFailedResult("رنگ مورد نظر پیدا نشد.");

        color.IsDeleted = true;
        color.IsActive = false;
        color.UpdatedBy = command.DeletedBy;

        await _context.SaveChangesAsync();

        return ResultOperation.ToSuccessResult("رنگ با موفقیت حذف شد.");
    }
}
