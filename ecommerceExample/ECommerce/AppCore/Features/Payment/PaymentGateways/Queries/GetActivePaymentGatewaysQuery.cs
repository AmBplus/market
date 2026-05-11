using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.PaymentGateways.Queries;

public class GetActivePaymentGatewaysQuery
{
}

public class GetActivePaymentGatewaysHandler
{
    private readonly ECommerceDbContext _context;
    public GetActivePaymentGatewaysHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<PaymentGatewayDto>>> Handle(GetActivePaymentGatewaysQuery query, CancellationToken ct)
    {
        var gateways = await _context.PaymentGateways
            .AsNoTracking()
            .Where(g => g.IsActive)
            .OrderBy(g => g.Priority)
            .Select(g => new PaymentGatewayDto
            {
                Id = g.Id,
                Name = g.Name,
                Code = g.Code,
                Priority = g.Priority,
                IsActive = g.IsActive
            })
            .ToListAsync(ct);

        return ResultOperation<List<PaymentGatewayDto>>.ToSuccessResult(gateways);
    }
}
