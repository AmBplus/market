using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.ColorCommands.Delete
{
    public record ColorDeleteCommand(
    long Id,
    long? DeletedBy
);
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
                .FirstOrDefaultAsync(c => c.Id == command.Id && !c.IsDelete);

            if (color is null)
                return ResultOperation.ToFailedResult("رنگ مورد نظر پیدا نشد.");

            // Soft-delete: فعال‌سازی علامت حذف و غیرفعال‌سازی
            color.IsDelete = true;
            color.IsActive = false;
            color.UpdateBy = command.DeletedBy ?? 0;
            color.UpdatedAt = DateTime.UtcNow;
            color.UpdatedBy = command.DeletedBy ?? 0;

            await _context.SaveChangesAsync();

            return ResultOperation.ToSuccessResult("رنگ با موفقیت حذف شد.");
        }
    }
}
