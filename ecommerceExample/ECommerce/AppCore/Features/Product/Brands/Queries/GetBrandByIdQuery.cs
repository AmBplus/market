using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Brands.Queries;

public class GetBrandByIdQuery
{
    public long Id { get; set; }
}

public class BrandDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class GetBrandByIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetBrandByIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<BrandDto>> Handle(GetBrandByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<BrandDto>.ToFailedResult("شناسه برند نامعتبر است");

        var brand = await _context.Brands
            .AsNoTracking()
            .Where(b => b.Id == query.Id)
            .Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Slug = b.Slug,
                LogoUrl = b.LogoUrl,
                Description = b.Description,
                IsActive = b.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (brand is null)
            return ResultOperation<BrandDto>.ToFailedResult("برند یافت نشد");

        return ResultOperation<BrandDto>.ToSuccessResult(brand);
    }
}
