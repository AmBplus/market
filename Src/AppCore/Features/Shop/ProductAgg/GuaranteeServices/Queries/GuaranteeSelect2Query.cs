using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Queries.GetGuaranteesSelect2Query;

public class GetGuaranteesSelect2Query
{
    public string? SearchTitle { get; set; }
}

public class GuaranteeSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class GetGuaranteesSelect2QueryHandler
{
    private readonly AppDbContext _context;

    public GetGuaranteesSelect2QueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<List<GuaranteeSelect2Dto>>> Handle(GetGuaranteesSelect2Query query, CancellationToken cancellationToken)
    {
        var baseQuery = _context.Guarantees
            .AsNoTracking()
            .Where(g => !g.IsDeleted && g.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTitle))
            baseQuery = baseQuery.Where(g => g.Title.Contains(query.SearchTitle));

        var guarantees = await baseQuery.Select(g => new GuaranteeSelect2Dto
        {
            Id = g.Id,
            Text = g.Title
        }).ToListAsync(cancellationToken);

        return ResultOperation<List<GuaranteeSelect2Dto>>.ToSuccessResult(guarantees);
    }
}
