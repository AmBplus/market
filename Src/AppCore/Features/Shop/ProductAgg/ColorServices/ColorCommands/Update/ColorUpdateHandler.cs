using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.ColorCommands.Update
{
    public record ColorUpdateCommand(
    long Id,
    string? Title = null,
    string? ColorCode = null,
    long? UpdateBy = null
);
    public class ColorUpdateHandler
    {
        private readonly AppDbContext _context;

        public ColorUpdateHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(ColorUpdateCommand command)
        {
            var color = await _context.Colors
                .FirstOrDefaultAsync(c => c.Id == command.Id && !c.IsDelete);

            if (color is null)
                return ResultOperation.ToFailedResult("رنگ مورد نظر یافت نشد.");

            // به‌روزرسانی فیلدها اگر مقدار جدید دریافت شده باشد
            if (!string.IsNullOrWhiteSpace(command.Title))
                color.Title = command.Title;

            if (!string.IsNullOrWhiteSpace(command.ColorCode))
                color.ColorCode = command.ColorCode;

            color.UpdateBy = command.UpdateBy ?? 0;
            color.UpdateAt = DateTime.UtcNow;
            color.UpdatedAt = DateTime.UtcNow;
            color.IsUpdate = true;

            await _context.SaveChangesAsync();

            return ResultOperation.ToSuccessResult("رنگ با موفقیت به‌روزرسانی شد.");
        }
    }
}
