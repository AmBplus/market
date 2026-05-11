using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.PaymentGateways.Queries;

public class GetPaymentGatewaysQuery
{
}

public class PaymentGatewayDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}

public class GetPaymentGatewaysHandler
{
    private readonly ECommerceDbContext _context;
    public GetPaymentGatewaysHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<PaymentGatewayDto>>> Handle(GetPaymentGatewaysQuery query, CancellationToken ct)
    {
        var gateways = await _context.PaymentGateways
            .AsNoTracking()
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
