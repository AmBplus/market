using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shipping.OrderDeliveries.Queries;

public class GetDeliveryByOrderIdQuery
{
    public long OrderId { get; set; }
}

public class OrderDeliveryDto
{
    public long Id { get; set; }
    public string DeliveryMethodName { get; set; } = string.Empty;
    public DeliveryStatus Status { get; set; }
    public string? TrackingCode { get; set; }
    public string? DeliveryAddressJson { get; set; }
}

public class GetDeliveryByOrderIdHandler
{
    private readonly AppDbContext _context;
    public GetDeliveryByOrderIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<OrderDeliveryDto>> Handle(GetDeliveryByOrderIdQuery query, CancellationToken ct)
    {
        if (query.OrderId <= 0)
            return ResultOperation<OrderDeliveryDto>.ToFailedResult("شناسه سفارش نامعتبر است");

        var delivery = await _context.OrderDeliveries
            .AsNoTracking()
            .Where(od => od.OrderId == query.OrderId)
            .Select(od => new OrderDeliveryDto
            {
                Id = od.Id,
                DeliveryMethodName = od.DeliveryMethod.Name,
                Status = od.Status,
                TrackingCode = od.TrackingCode,
                DeliveryAddressJson = od.DeliveryAddressJson
            })
            .FirstOrDefaultAsync(ct);

        if (delivery is null)
            return ResultOperation<OrderDeliveryDto>.ToFailedResult("اطلاعات تحویل سفارش یافت نشد");

        return ResultOperation<OrderDeliveryDto>.ToSuccessResult(delivery);
    }
}
