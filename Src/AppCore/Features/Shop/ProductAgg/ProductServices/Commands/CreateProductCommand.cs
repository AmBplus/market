using AppCore.Data;
using AppCore.Domains.Product;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Products.Commands;

public class CreateProductAttributeDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Group { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateProductCommand
{
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public long? BrandId { get; set; }
    public long? VendorId { get; set; }
    public string? ShippingConstraintsJson { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public List<long> CategoryIds { get; set; } = new();
    public List<CreateProductAttributeDto> Attributes { get; set; } = new();
}

public class CreateProductHandler
{
    private readonly AppDbContext _context;
    public CreateProductHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateProductCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.ProductCode))
            return ResultOperation.ToFailedResult("کد محصول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام محصول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Slug))
            return ResultOperation.ToFailedResult("اسلاگ محصول الزامی است");

        var codeExists = await _context.Products.AnyAsync(p => p.ProductCode == command.ProductCode, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد محصول تکراری است");

        var slugExists = await _context.Products.AnyAsync(p => p.Slug == command.Slug, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ محصول تکراری است");

        if (command.BrandId.HasValue)
        {
            var brandExists = await _context.Brands.AnyAsync(b => b.Id == command.BrandId.Value, ct);
            if (!brandExists)
                return ResultOperation.ToFailedResult("برند انتخاب شده معتبر نیست");
        }

        var product = new Domains.Product.Product
        {
            ProductCode = command.ProductCode,
            Name = command.Name,
            Slug = command.Slug,
            Description = command.Description,
            ShortDescription = command.ShortDescription,
            BrandId = command.BrandId,
            VendorId = command.VendorId,
            ShippingConstraintsJson = command.ShippingConstraintsJson,
            MetaTitle = command.MetaTitle,
            MetaDescription = command.MetaDescription,
            IsActive = command.IsActive,
            IsFeatured = command.IsFeatured
        };

        _context.Add(product);
        await _context.SaveChangesAsync(ct);

        if (command.CategoryIds is { Count: > 0 })
        {
            var validCategories = await _context.Categories
                .Where(c => command.CategoryIds.Contains(c.Id) && c.IsActive)
                .Select(c => c.Id)
                .ToListAsync(ct);

            var displayOrder = 0;
            foreach (var categoryId in validCategories)
            {
                _context.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId,
                    DisplayOrder = displayOrder++
                });
            }
        }

        if (command.Attributes is { Count: > 0 })
        {
            foreach (var attr in command.Attributes)
            {
                if (string.IsNullOrWhiteSpace(attr.Key) || string.IsNullOrWhiteSpace(attr.Value))
                    continue;

                _context.Add(new ProductAttribute
                {
                    ProductId = product.Id,
                    Key = attr.Key,
                    Value = attr.Value,
                    Group = attr.Group,
                    DisplayOrder = attr.DisplayOrder
                });
            }
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("محصول با موفقیت ایجاد شد");
    }
}
