using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.Payments.Commands;

public class RefundPaymentCommand
{
    public long PaymentId { get; set; }
    public string? Reason { get; set; }
}

public class RefundPaymentHandler
{
    private readonly ECommerceDbContext _context;
    public RefundPaymentHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(RefundPaymentCommand command, CancellationToken ct)
    {
        if (command.PaymentId <= 0)
            return ResultOperation.ToFailedResult("شناسه پرداخت نامعتبر است");

        var payment = await _context.Payments.FindAsync(new object[] { command.PaymentId }, ct);
        if (payment is null)
            return ResultOperation.ToFailedResult("پرداخت یافت نشد");

        if (payment.Status != PaymentStatus.Succeeded)
            return ResultOperation.ToFailedResult("فقط پرداخت‌های موفق قابل بازگشت هستند");

        payment.Status = PaymentStatus.Refunded;
        payment.BankMessage = command.Reason ?? payment.BankMessage;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("بازگشت وجه با موفقیت ثبت شد");
    }
}
