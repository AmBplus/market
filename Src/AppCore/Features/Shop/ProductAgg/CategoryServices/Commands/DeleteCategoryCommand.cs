using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Categories.Commands;

public class DeleteCategoryCommand
{
    public long Id { get; set; }
}

public class DeleteCategoryHandler
{
    private readonly AppDbContext _context;
    public DeleteCategoryHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteCategoryCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه دسته‌بندی نامعتبر است");

        var category = await _context.Categories.FindAsync(new object[] { command.Id }, ct);
        if (category is null)
            return ResultOperation.ToFailedResult("دسته‌بندی یافت نشد");

        var hasChildren = await _context.Categories.AnyAsync(c => c.ParentId == command.Id, ct);
        if (hasChildren)
            return ResultOperation.ToFailedResult("این دسته‌بندی دارای زیردسته است و قابل حذف نیست");

        var hasProducts = await _context.ProductCategories.AnyAsync(pc => pc.CategoryId == command.Id, ct);
        if (hasProducts)
            return ResultOperation.ToFailedResult("این دسته‌بندی دارای محصول است و قابل حذف نیست");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("دسته‌بندی با موفقیت حذف شد");
    }
}
