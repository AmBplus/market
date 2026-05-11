using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Payment.Payments.Queries;

public class GetPaymentByIdQuery
{
    public long Id { get; set; }
}

public class PaymentDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string GatewayName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionRef { get; set; }
    public string? TrackingCode { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetPaymentByIdHandler
{
    private readonly AppDbContext _context;
    public GetPaymentByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<PaymentDto>> Handle(GetPaymentByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<PaymentDto>.ToFailedResult("شناسه پرداخت نامعتبر است");

        var payment = await _context.Payments
            .AsNoTracking()
            .Where(p => p.Id == query.Id)
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
            .FirstOrDefaultAsync(ct);

        if (payment is null)
            return ResultOperation<PaymentDto>.ToFailedResult("پرداخت یافت نشد");

        return ResultOperation<PaymentDto>.ToSuccessResult(payment);
    }
}
