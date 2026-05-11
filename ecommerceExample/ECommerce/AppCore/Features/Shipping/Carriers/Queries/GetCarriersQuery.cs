using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.Carriers.Queries;

public class GetCarriersQuery
{
}

public class CarrierDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? TrackingUrlTemplate { get; set; }
    public bool IsActive { get; set; }
}

public class GetCarriersHandler
{
    private readonly ECommerceDbContext _context;
    public GetCarriersHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<CarrierDto>>> Handle(GetCarriersQuery query, CancellationToken ct)
    {
        var carriers = await _context.Carriers
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CarrierDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                TrackingUrlTemplate = c.TrackingUrlTemplate,
                IsActive = c.IsActive
            })
            .ToListAsync(ct);

        return ResultOperation<List<CarrierDto>>.ToSuccessResult(carriers);
    }
}
