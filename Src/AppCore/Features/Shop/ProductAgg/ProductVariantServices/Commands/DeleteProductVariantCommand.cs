using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.ProductVariants.Commands;

public class DeleteProductVariantCommand
{
    public long Id { get; set; }
}

public class DeleteProductVariantHandler
{
    private readonly AppDbContext _context;
    public DeleteProductVariantHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteProductVariantCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع نامعتبر است");

        var variant = await _context.ProductVariants.FindAsync(new object[] { command.Id }, ct);
        if (variant is null)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var hasActivePrices = await _context.Prices
            .AnyAsync(p => p.ProductVariantId == command.Id && p.EndedAt == null, ct);
        if (hasActivePrices)
            return ResultOperation.ToFailedResult("این تنوع دارای قیمت فعال است و قابل حذف نیست");

        variant.IsDeleted = true;
        variant.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تنوع محصول با موفقیت حذف شد");
    }
}
