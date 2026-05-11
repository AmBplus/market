using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.OrderDeliveries.Commands;

public class UpdateDeliveryStatusCommand
{
    public long OrderDeliveryId { get; set; }
    public DeliveryStatus Status { get; set; }
    public string? TrackingCode { get; set; }
    public string? Note { get; set; }
}

public class UpdateDeliveryStatusHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateDeliveryStatusHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateDeliveryStatusCommand command, CancellationToken ct)
    {
        if (command.OrderDeliveryId <= 0)
            return ResultOperation.ToFailedResult("شناسه تحویل سفارش نامعتبر است");

        var delivery = await _context.OrderDeliveries.FindAsync(new object[] { command.OrderDeliveryId }, ct);
        if (delivery is null)
            return ResultOperation.ToFailedResult("تحویل سفارش یافت نشد");

        // Validate status transition
        var validTransition = delivery.Status switch
        {
            DeliveryStatus.Pending => command.Status is DeliveryStatus.InTransit or DeliveryStatus.Failed,
            DeliveryStatus.InTransit => command.Status is DeliveryStatus.Delivered or DeliveryStatus.Failed,
            DeliveryStatus.Delivered => false,
            DeliveryStatus.Failed => command.Status is DeliveryStatus.InTransit,
            _ => false
        };

        if (!validTransition)
            return ResultOperation.ToFailedResult($"تغییر وضعیت از {delivery.Status} به {command.Status} مجاز نیست");

        delivery.Status = command.Status;

        if (!string.IsNullOrWhiteSpace(command.TrackingCode))
            delivery.TrackingCode = command.TrackingCode;

        if (!string.IsNullOrWhiteSpace(command.Note))
            delivery.Note = command.Note;

        if (command.Status == DeliveryStatus.InTransit)
            delivery.ShippedAt = DateTime.UtcNow;

        if (command.Status == DeliveryStatus.Delivered)
            delivery.DeliveredAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("وضعیت تحویل با موفقیت بروزرسانی شد");
    }
}
