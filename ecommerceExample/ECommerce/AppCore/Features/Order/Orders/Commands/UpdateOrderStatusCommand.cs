using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Order;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Orders.Commands;

public class UpdateOrderStatusCommand
{
    public long OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
    public string? Note { get; set; }
    public long? ChangedBy { get; set; }
}

public class UpdateOrderStatusHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateOrderStatusHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateOrderStatusCommand command, CancellationToken ct)
    {
        if (command.OrderId <= 0)
            return ResultOperation.ToFailedResult("شناسه سفارش نامعتبر است");

        var order = await _context.Orders.FindAsync(new object[] { command.OrderId }, ct);
        if (order is null)
            return ResultOperation.ToFailedResult("سفارش یافت نشد");

        // Validate status transition
        var validTransition = order.Status switch
        {
            OrderStatus.Pending => command.NewStatus is OrderStatus.Paid or OrderStatus.Cancelled,
            OrderStatus.Paid => command.NewStatus is OrderStatus.Processing or OrderStatus.Cancelled,
            OrderStatus.Processing => command.NewStatus is OrderStatus.Shipped or OrderStatus.Cancelled,
            OrderStatus.Shipped => command.NewStatus is OrderStatus.Delivered,
            OrderStatus.Delivered => false,
            OrderStatus.Cancelled => false,
            OrderStatus.Refunded => false,
            _ => false
        };

        if (!validTransition)
            return ResultOperation.ToFailedResult($"تغییر وضعیت از {order.Status} به {command.NewStatus} مجاز نیست");

        var previousStatus = order.Status;
        order.Status = command.NewStatus;

        if (command.NewStatus == OrderStatus.Delivered)
            order.CompletedAt = DateTime.UtcNow;

        var statusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            FromStatus = previousStatus,
            ToStatus = command.NewStatus,
            Note = command.Note,
            ChangedBy = command.ChangedBy
        };
        _context.Add(statusHistory);

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("وضعیت سفارش با موفقیت بروزرسانی شد");
    }
}
