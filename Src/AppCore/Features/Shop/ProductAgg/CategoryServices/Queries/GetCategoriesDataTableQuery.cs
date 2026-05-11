using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Categories.Queries;

public class GetCategoriesDataTableQuery : DatatableFullRequest
{
    public string? SearchName { get; set; }
    public string? SearchCode { get; set; }
    public long? SearchParentId { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class CategoryListDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ParentName { get; set; }
    public int ProductCount { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

public class GetCategoriesDataTableHandler
{
    private readonly AppDbContext _context;
    public GetCategoriesDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<CategoryListDto>>> Handle(GetCategoriesDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(c => c.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(c => c.Code.Contains(query.SearchCode));

        if (query.SearchParentId.HasValue)
            baseQuery = baseQuery.Where(c => c.ParentId == query.SearchParentId.Value);

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(c => c.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(c => new CategoryListDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                ParentName = c.Parent != null ? c.Parent.Name : null,
                ProductCount = c.ProductCategories.Count,
                IsActive = c.IsActive,
                DisplayOrder = c.DisplayOrder
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<CategoryListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<CategoryListDto>>.ToSuccessResult(response);
    }
}
