using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Order;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.Payments.Commands;

public class CreatePaymentCommand
{
    public long OrderId { get; set; }
    public long GatewayId { get; set; }
    public string CallbackUrl { get; set; } = string.Empty;
}

public class CreatePaymentHandler
{
    private readonly ECommerceDbContext _context;
    public CreatePaymentHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<long>> Handle(CreatePaymentCommand command, CancellationToken ct)
    {
        if (command.OrderId <= 0)
            return ResultOperation<long>.ToFailedResult("شناسه سفارش نامعتبر است");

        if (command.GatewayId <= 0)
            return ResultOperation<long>.ToFailedResult("شناسه درگاه پرداخت نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.CallbackUrl))
            return ResultOperation<long>.ToFailedResult("آدرس بازگشت الزامی است");

        var order = await _context.Orders.FindAsync(new object[] { command.OrderId }, ct);
        if (order is null)
            return ResultOperation<long>.ToFailedResult("سفارش یافت نشد");

        var gateway = await _context.PaymentGateways
            .FirstOrDefaultAsync(g => g.Id == command.GatewayId && g.IsActive, ct);
        if (gateway is null)
            return ResultOperation<long>.ToFailedResult("درگاه پرداخت یافت نشد یا غیرفعال است");

        var payment = new Domain.Payment.Payment
        {
            OrderId = command.OrderId,
            GatewayId = command.GatewayId,
            Amount = order.TotalAmount,
            CurrencyId = order.CurrencyId,
            Status = PaymentStatus.Created,
            CallbackUrl = command.CallbackUrl
        };

        _context.Add(payment);
        await _context.SaveChangesAsync(ct);

        return ResultOperation<long>.ToSuccessResult(payment.Id, "پرداخت با موفقیت ایجاد شد");
    }
}
