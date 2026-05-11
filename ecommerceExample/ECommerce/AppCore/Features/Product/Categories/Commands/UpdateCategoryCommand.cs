using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Categories.Commands;

public class UpdateCategoryCommand
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCategoryHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateCategoryHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateCategoryCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه دسته‌بندی نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد دسته‌بندی الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام دسته‌بندی الزامی است");

        if (string.IsNullOrWhiteSpace(command.Slug))
            return ResultOperation.ToFailedResult("اسلاگ دسته‌بندی الزامی است");

        var category = await _context.Categories.FindAsync(new object[] { command.Id }, ct);
        if (category is null)
            return ResultOperation.ToFailedResult("دسته‌بندی یافت نشد");

        var codeExists = await _context.Categories.AnyAsync(c => c.Code == command.Code && c.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد دسته‌بندی تکراری است");

        var slugExists = await _context.Categories.AnyAsync(c => c.Slug == command.Slug && c.Id != command.Id, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ دسته‌بندی تکراری است");

        if (command.ParentId.HasValue)
        {
            if (command.ParentId.Value == command.Id)
                return ResultOperation.ToFailedResult("دسته‌بندی نمی‌تواند والد خودش باشد");

            var parentExists = await _context.Categories.AnyAsync(c => c.Id == command.ParentId.Value, ct);
            if (!parentExists)
                return ResultOperation.ToFailedResult("دسته‌بندی والد یافت نشد");
        }

        category.ParentId = command.ParentId;
        category.Code = command.Code;
        category.Name = command.Name;
        category.Slug = command.Slug;
        category.Description = command.Description;
        category.ImageUrl = command.ImageUrl;
        category.DisplayOrder = command.DisplayOrder;
        category.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("دسته‌بندی با موفقیت ویرایش شد");
    }
}
