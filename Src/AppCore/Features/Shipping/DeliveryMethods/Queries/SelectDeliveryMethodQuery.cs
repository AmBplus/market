using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shipping.DeliveryMethods.Queries;

public class SelectDeliveryMethodQuery
{
    public string? SearchName { get; set; }
}

public class DeliveryMethodSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectDeliveryMethodHandler
{
    private readonly AppDbContext _context;
    public SelectDeliveryMethodHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<DeliveryMethodSelect2Dto>>> Handle(SelectDeliveryMethodQuery query, CancellationToken ct)
    {
        var baseQuery = _context.DeliveryMethods.AsNoTracking().Where(dm => dm.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(dm => dm.Name.Contains(query.SearchName));

        var result = await baseQuery
            .Select(dm => new DeliveryMethodSelect2Dto
            {
                Id = dm.Id,
                Text = dm.Name + " (" + dm.DeliveryType.ToString() + ")"
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<DeliveryMethodSelect2Dto>>.ToSuccessResult(result);
    }
}
