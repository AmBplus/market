using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Products.Queries;

public class GetProductByIdQuery
{
    public long Id { get; set; }
}

public class ProductAttributeDto
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Group { get; set; }
    public int DisplayOrder { get; set; }
}

public class ProductDto
{
    public long Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public long? BrandId { get; set; }
    public string? BrandName { get; set; }
    public long? VendorId { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public List<long> CategoryIds { get; set; } = new();
    public List<ProductAttributeDto> Attributes { get; set; } = new();
}

public class GetProductByIdHandler
{
    private readonly AppDbContext _context;
    public GetProductByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<ProductDto>.ToFailedResult("شناسه محصول نامعتبر است");

        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == query.Id && !p.IsDeleted)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                ShortDescription = p.ShortDescription,
                BrandId = p.BrandId,
                BrandName = p.Brand != null ? p.Brand.Name : null,
                VendorId = p.VendorId,
                IsActive = p.IsActive,
                IsFeatured = p.IsFeatured,
                CategoryIds = p.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                Attributes = p.Attributes.Select(a => new ProductAttributeDto
                {
                    Id = a.Id,
                    Key = a.Key,
                    Value = a.Value,
                    Group = a.Group,
                    DisplayOrder = a.DisplayOrder
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (product is null)
            return ResultOperation<ProductDto>.ToFailedResult("محصول یافت نشد");

        return ResultOperation<ProductDto>.ToSuccessResult(product);
    }
}
