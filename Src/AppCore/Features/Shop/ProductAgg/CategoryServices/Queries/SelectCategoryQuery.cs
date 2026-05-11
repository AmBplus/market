using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Categories.Queries;

public class SelectCategoryQuery
{
    public string? SearchName { get; set; }
    public string? SearchCode { get; set; }
}

public class CategorySelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectCategoryHandler
{
    private readonly AppDbContext _context;
    public SelectCategoryHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<CategorySelect2Dto>>> Handle(SelectCategoryQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Categories.AsNoTracking().Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(c => c.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(c => c.Code.Contains(query.SearchCode));

        var result = await baseQuery
            .Select(c => new CategorySelect2Dto
            {
                Id = c.Id,
                Text = c.Code + " - " + c.Name
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<CategorySelect2Dto>>.ToSuccessResult(result);
    }
}
