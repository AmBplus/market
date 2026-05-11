using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Product;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Categories.Commands;

public class CreateCategoryCommand
{
    public long? ParentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateCategoryHandler
{
    private readonly ECommerceDbContext _context;
    public CreateCategoryHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateCategoryCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد دسته‌بندی الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام دسته‌بندی الزامی است");

        if (string.IsNullOrWhiteSpace(command.Slug))
            return ResultOperation.ToFailedResult("اسلاگ دسته‌بندی الزامی است");

        var codeExists = await _context.Categories.AnyAsync(c => c.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد دسته‌بندی تکراری است");

        var slugExists = await _context.Categories.AnyAsync(c => c.Slug == command.Slug, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ دسته‌بندی تکراری است");

        if (command.ParentId.HasValue)
        {
            var parentExists = await _context.Categories.AnyAsync(c => c.Id == command.ParentId.Value, ct);
            if (!parentExists)
                return ResultOperation.ToFailedResult("دسته‌بندی والد یافت نشد");
        }

        var category = new Category
        {
            ParentId = command.ParentId,
            Code = command.Code,
            Name = command.Name,
            Slug = command.Slug,
            Description = command.Description,
            ImageUrl = command.ImageUrl,
            DisplayOrder = command.DisplayOrder,
            IsActive = command.IsActive
        };

        _context.Add(category);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("دسته‌بندی با موفقیت ایجاد شد");
    }
}
