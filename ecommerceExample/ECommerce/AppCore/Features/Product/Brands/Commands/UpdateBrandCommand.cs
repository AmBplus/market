using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Brands.Commands;

public class UpdateBrandCommand
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateBrandHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateBrandHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateBrandCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه برند نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام برند الزامی است");

        if (string.IsNullOrWhiteSpace(command.Slug))
            return ResultOperation.ToFailedResult("اسلاگ برند الزامی است");

        var brand = await _context.Brands.FindAsync(new object[] { command.Id }, ct);
        if (brand is null)
            return ResultOperation.ToFailedResult("برند یافت نشد");

        var nameExists = await _context.Brands.AnyAsync(b => b.Name == command.Name && b.Id != command.Id, ct);
        if (nameExists)
            return ResultOperation.ToFailedResult("نام برند تکراری است");

        var slugExists = await _context.Brands.AnyAsync(b => b.Slug == command.Slug && b.Id != command.Id, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ برند تکراری است");

        brand.Name = command.Name;
        brand.Slug = command.Slug;
        brand.LogoUrl = command.LogoUrl;
        brand.Description = command.Description;
        brand.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("برند با موفقیت ویرایش شد");
    }
}
