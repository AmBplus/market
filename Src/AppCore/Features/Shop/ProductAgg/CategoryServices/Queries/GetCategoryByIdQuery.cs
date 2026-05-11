using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Categories.Queries;

public class GetCategoryByIdQuery
{
    public long Id { get; set; }
}

public class CategoryDto
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

public class GetCategoryByIdHandler
{
    private readonly AppDbContext _context;
    public GetCategoryByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<CategoryDto>> Handle(GetCategoryByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<CategoryDto>.ToFailedResult("شناسه دسته‌بندی نامعتبر است");

        var category = await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == query.Id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Code = c.Code,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (category is null)
            return ResultOperation<CategoryDto>.ToFailedResult("دسته‌بندی یافت نشد");

        return ResultOperation<CategoryDto>.ToSuccessResult(category);
    }
}
