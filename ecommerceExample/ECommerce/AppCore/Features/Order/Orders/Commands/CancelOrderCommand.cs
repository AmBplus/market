using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Domain.Order;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Orders.Commands;

public class CancelOrderCommand
{
    public long OrderId { get; set; }
    public string? Reason { get; set; }
}

public class CancelOrderHandler
{
    private readonly ECommerceDbContext _context;
    public CancelOrderHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CancelOrderCommand command, CancellationToken ct)
    {
        if (command.OrderId <= 0)
            return ResultOperation.ToFailedResult("شناسه سفارش نامعتبر است");

        var order = await _context.Orders
            .Include(o => o.StockReservations)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);

        if (order is null)
            return ResultOperation.ToFailedResult("سفارش یافت نشد");

        if (order.Status != OrderStatus.Pending)
            return ResultOperation.ToFailedResult("فقط سفارشات در وضعیت انتظار قابل لغو هستند");

        // Update order status
        order.Status = OrderStatus.Cancelled;

        // Release stock reservations
        foreach (var reservation in order.StockReservations.Where(r => !r.IsReleased))
        {
            reservation.IsReleased = true;
            reservation.ReleasedAt = DateTime.UtcNow;

            var stockLedger = new StockLedger
            {
                ProductVariantId = reservation.ProductVariantId,
                WarehouseId = reservation.WarehouseId,
                QuantityChange = reservation.Quantity,
                Reason = StockChangeReason.ReservationReleased,
                ReferenceId = order.Id,
                Description = $"آزادسازی رزرو سفارش {order.OrderNumber}"
            };
            _context.Add(stockLedger);
        }

        // Add order status history
        var statusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            FromStatus = OrderStatus.Pending,
            ToStatus = OrderStatus.Cancelled,
            Note = command.Reason ?? "لغو سفارش"
        };
        _context.Add(statusHistory);

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("سفارش با موفقیت لغو شد");
    }
}
