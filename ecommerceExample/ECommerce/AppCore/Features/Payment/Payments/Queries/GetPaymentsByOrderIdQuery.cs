using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.Payments.Queries;

public class GetPaymentsByOrderIdQuery
{
    public long OrderId { get; set; }
}

public class GetPaymentsByOrderIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetPaymentsByOrderIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<PaymentDto>>> Handle(GetPaymentsByOrderIdQuery query, CancellationToken ct)
    {
        if (query.OrderId <= 0)
            return ResultOperation<List<PaymentDto>>.ToFailedResult("شناسه سفارش نامعتبر است");

        var payments = await _context.Payments
            .AsNoTracking()
            .Where(p => p.OrderId == query.OrderId)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                GatewayName = p.Gateway.Name,
                Amount = p.Amount,
                Status = p.Status,
                TransactionRef = p.TransactionRef,
                TrackingCode = p.TrackingCode,
                PaidAt = p.PaidAt,
                CreatedAt = p.CreatedAt
            })
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

        return ResultOperation<List<PaymentDto>>.ToSuccessResult(payments);
    }
}
