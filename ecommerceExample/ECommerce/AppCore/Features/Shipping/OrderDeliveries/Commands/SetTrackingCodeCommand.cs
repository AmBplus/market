using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.OrderDeliveries.Commands;

public class SetTrackingCodeCommand
{
    public long OrderDeliveryId { get; set; }
    public string TrackingCode { get; set; } = string.Empty;
}

public class SetTrackingCodeHandler
{
    private readonly ECommerceDbContext _context;
    public SetTrackingCodeHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(SetTrackingCodeCommand command, CancellationToken ct)
    {
        if (command.OrderDeliveryId <= 0)
            return ResultOperation.ToFailedResult("شناسه تحویل سفارش نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.TrackingCode))
            return ResultOperation.ToFailedResult("کد رهگیری الزامی است");

        var delivery = await _context.OrderDeliveries.FindAsync(new object[] { command.OrderDeliveryId }, ct);
        if (delivery is null)
            return ResultOperation.ToFailedResult("تحویل سفارش یافت نشد");

        delivery.TrackingCode = command.TrackingCode;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("کد رهگیری با موفقیت ثبت شد");
    }
}
