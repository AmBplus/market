using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Products.Commands;

public class DeleteProductCommand
{
    public long Id { get; set; }
}

public class DeleteProductHandler
{
    private readonly AppDbContext _context;
    public DeleteProductHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه محصول نامعتبر است");

        var product = await _context.Products.FindAsync(new object[] { command.Id }, ct);
        if (product is null)
            return ResultOperation.ToFailedResult("محصول یافت نشد");

        var hasActiveVariants = await _context.ProductVariants
            .AnyAsync(pv => pv.ProductId == command.Id && !pv.IsDeleted, ct);
        if (hasActiveVariants)
            return ResultOperation.ToFailedResult("این محصول دارای تنوع فعال است و قابل حذف نیست");

        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("محصول با موفقیت حذف شد");
    }
}
