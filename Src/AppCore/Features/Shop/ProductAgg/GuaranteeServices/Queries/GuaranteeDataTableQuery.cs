using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Queries.GetGuaranteesDataTableQuery;

public class GetGuaranteesDataTableQuery : DatatableFullRequest
{
    public string? SearchTitle { get; set; }
}

public class GuaranteeListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetGuaranteesDataTableHandler
{
    private readonly AppDbContext _context;

    public GetGuaranteesDataTableHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<DataTableResponse<GuaranteeListDto>>> Handle(GetGuaranteesDataTableQuery query, CancellationToken cancellationToken)
    {
        var baseQuery = _context.Guarantees
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTitle))
            baseQuery = baseQuery.Where(g => g.Title.Contains(query.SearchTitle));

        var filteredCount = await baseQuery.CountAsync(cancellationToken);

        var dtoQuery = baseQuery.Select(g => new GuaranteeListDto
        {
            Id = g.Id,
            Title = g.Title,
            IsActive = g.IsActive,
            CreatedAt = g.CreatedAt
        });

        var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

        var guarantees = await orderedQuery
            .Skip(query.start)
            .Take(query.length)
            .ToListAsync(cancellationToken);

        var response = new DataTableResponse<GuaranteeListDto>
        {
            Draw = query.draw,
            RecordsTotal = filteredCount,
            RecordsFiltered = filteredCount,
            Data = guarantees
        };

        return ResultOperation<DataTableResponse<GuaranteeListDto>>.ToSuccessResult(response);
    }
}
