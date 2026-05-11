using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.DeliveryMethods.Queries;

public class GetDeliveryMethodsQuery
{
}

public class DeliveryMethodDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeliveryType DeliveryType { get; set; }
    public long? CarrierId { get; set; }
    public string? CarrierName { get; set; }
    public decimal Price { get; set; }
    public long CurrencyId { get; set; }
    public int? EstimatedDays { get; set; }
    public string? Description { get; set; }
    public string? ConstraintsJson { get; set; }
    public DigitalContentType? DigitalContentType { get; set; }
    public bool IsActive { get; set; }
}

public class GetDeliveryMethodsHandler
{
    private readonly ECommerceDbContext _context;
    public GetDeliveryMethodsHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<DeliveryMethodDto>>> Handle(GetDeliveryMethodsQuery query, CancellationToken ct)
    {
        var methods = await _context.DeliveryMethods
            .AsNoTracking()
            .OrderBy(dm => dm.Name)
            .Select(dm => new DeliveryMethodDto
            {
                Id = dm.Id,
                Name = dm.Name,
                DeliveryType = dm.DeliveryType,
                CarrierId = dm.CarrierId,
                CarrierName = dm.Carrier != null ? dm.Carrier.Name : null,
                Price = dm.Price,
                CurrencyId = dm.CurrencyId,
                EstimatedDays = dm.EstimatedDays,
                Description = dm.Description,
                ConstraintsJson = dm.ConstraintsJson,
                DigitalContentType = dm.DigitalContentType,
                IsActive = dm.IsActive
            })
            .ToListAsync(ct);

        return ResultOperation<List<DeliveryMethodDto>>.ToSuccessResult(methods);
    }
}
