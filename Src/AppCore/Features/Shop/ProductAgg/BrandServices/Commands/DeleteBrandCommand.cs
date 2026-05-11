using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Brands.Commands;

public class DeleteBrandCommand
{
    public long Id { get; set; }
}

public class DeleteBrandHandler
{
    private readonly AppDbContext _context;
    public DeleteBrandHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteBrandCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه برند نامعتبر است");

        var brand = await _context.Brands.FindAsync(new object[] { command.Id }, ct);
        if (brand is null)
            return ResultOperation.ToFailedResult("برند یافت نشد");

        var hasProducts = await _context.Products.AnyAsync(p => p.BrandId == command.Id && !p.IsDeleted, ct);
        if (hasProducts)
            return ResultOperation.ToFailedResult("این برند دارای محصول فعال است و قابل حذف نیست");

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("برند با موفقیت حذف شد");
    }
}
