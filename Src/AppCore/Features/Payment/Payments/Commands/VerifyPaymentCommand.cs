using AppCore.Data;
using AppCore.Domains.Order;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Payment.Payments.Commands;

public class VerifyPaymentCommand
{
    public long PaymentId { get; set; }
    public string TransactionRef { get; set; } = string.Empty;
    public string? BankMessage { get; set; }
}

public class VerifyPaymentHandler
{
    private readonly AppDbContext _context;
    public VerifyPaymentHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(VerifyPaymentCommand command, CancellationToken ct)
    {
        if (command.PaymentId <= 0)
            return ResultOperation.ToFailedResult("شناسه پرداخت نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.TransactionRef))
            return ResultOperation.ToFailedResult("شماره تراکنش الزامی است");

        var payment = await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == command.PaymentId, ct);

        if (payment is null)
            return ResultOperation.ToFailedResult("پرداخت یافت نشد");

        if (payment.Status != PaymentStatus.Created && payment.Status != PaymentStatus.Pending)
            return ResultOperation.ToFailedResult("وضعیت پرداخت اجازه تایید نمی‌دهد");

        // Update payment
        payment.Status = PaymentStatus.Succeeded;
        payment.TransactionRef = command.TransactionRef;
        payment.BankMessage = command.BankMessage;
        payment.PaidAt = DateTime.UtcNow;

        // Update order status
        var order = payment.Order;
        var previousStatus = order.Status;
        order.Status = OrderStatus.Paid;

        // Add order status history
        var statusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            FromStatus = previousStatus,
            ToStatus = OrderStatus.Paid,
            Note = "پرداخت تایید شد"
        };
        _context.Add(statusHistory);

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("پرداخت با موفقیت تایید شد");
    }
}
