using AppCore.Data;
using AppCore.Domains.Product;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Brands.Commands;

public class CreateBrandCommand
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateBrandHandler
{
    private readonly AppDbContext _context;
    public CreateBrandHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateBrandCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام برند الزامی است");

        if (string.IsNullOrWhiteSpace(command.Slug))
            return ResultOperation.ToFailedResult("اسلاگ برند الزامی است");

        var nameExists = await _context.Brands.AnyAsync(b => b.Name == command.Name, ct);
        if (nameExists)
            return ResultOperation.ToFailedResult("نام برند تکراری است");

        var slugExists = await _context.Brands.AnyAsync(b => b.Slug == command.Slug, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ برند تکراری است");

        var brand = new Brand
        {
            Name = command.Name,
            Slug = command.Slug,
            LogoUrl = command.LogoUrl,
            Description = command.Description,
            IsActive = command.IsActive
        };

        _context.Add(brand);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("برند با موفقیت ایجاد شد");
    }
}
