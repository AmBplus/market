using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Product;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Products.Commands;

public class UpdateProductCommand
{
    public long Id { get; set; }
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
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public List<long> CategoryIds { get; set; } = new();
    public List<CreateProductAttributeDto> Attributes { get; set; } = new();
}

public class UpdateProductHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateProductHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه محصول نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.ProductCode))
            return ResultOperation.ToFailedResult("کد محصول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام محصول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Slug))
            return ResultOperation.ToFailedResult("اسلاگ محصول الزامی است");

        var product = await _context.Products.FindAsync(new object[] { command.Id }, ct);
        if (product is null)
            return ResultOperation.ToFailedResult("محصول یافت نشد");

        var codeExists = await _context.Products.AnyAsync(p => p.ProductCode == command.ProductCode && p.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد محصول تکراری است");

        var slugExists = await _context.Products.AnyAsync(p => p.Slug == command.Slug && p.Id != command.Id, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ محصول تکراری است");

        if (command.BrandId.HasValue)
        {
            var brandExists = await _context.Brands.AnyAsync(b => b.Id == command.BrandId.Value, ct);
            if (!brandExists)
                return ResultOperation.ToFailedResult("برند انتخاب شده معتبر نیست");
        }

        product.ProductCode = command.ProductCode;
        product.Name = command.Name;
        product.Slug = command.Slug;
        product.Description = command.Description;
        product.ShortDescription = command.ShortDescription;
        product.BrandId = command.BrandId;
        product.VendorId = command.VendorId;
        product.ShippingConstraintsJson = command.ShippingConstraintsJson;
        product.MetaTitle = command.MetaTitle;
        product.MetaDescription = command.MetaDescription;
        product.IsActive = command.IsActive;
        product.IsFeatured = command.IsFeatured;

        // Sync categories: remove old, add new
        var existingCategories = await _context.ProductCategories
            .Where(pc => pc.ProductId == command.Id)
            .ToListAsync(ct);
        _context.ProductCategories.RemoveRange(existingCategories);

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

        // Sync attributes: remove old, add new
        var existingAttributes = await _context.ProductAttributes
            .Where(pa => pa.ProductId == command.Id)
            .ToListAsync(ct);
        _context.ProductAttributes.RemoveRange(existingAttributes);

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

        return ResultOperation.ToSuccessResult("محصول با موفقیت ویرایش شد");
    }
}
