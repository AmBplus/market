using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.DeliveryMethods.Commands;

public class UpdateDeliveryMethodCommand
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeliveryType DeliveryType { get; set; }
    public long? CarrierId { get; set; }
    public decimal Price { get; set; }
    public long CurrencyId { get; set; }
    public int? EstimatedDays { get; set; }
    public string? Description { get; set; }
    public string? ConstraintsJson { get; set; }
    public DigitalContentType? DigitalContentType { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateDeliveryMethodHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateDeliveryMethodHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateDeliveryMethodCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه روش ارسال نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام روش ارسال الزامی است");

        if (command.Price < 0)
            return ResultOperation.ToFailedResult("قیمت ارسال نمی‌تواند منفی باشد");

        var deliveryMethod = await _context.DeliveryMethods.FindAsync(new object[] { command.Id }, ct);
        if (deliveryMethod is null)
            return ResultOperation.ToFailedResult("روش ارسال یافت نشد");

        if (command.CarrierId.HasValue)
        {
            var carrierExists = await _context.Carriers.AnyAsync(c => c.Id == command.CarrierId.Value && c.IsActive, ct);
            if (!carrierExists)
                return ResultOperation.ToFailedResult("شرکت حمل یافت نشد یا غیرفعال است");
        }

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == command.CurrencyId, ct);
        if (!currencyExists)
            return ResultOperation.ToFailedResult("واحد پول یافت نشد");

        deliveryMethod.Name = command.Name;
        deliveryMethod.DeliveryType = command.DeliveryType;
        deliveryMethod.CarrierId = command.CarrierId;
        deliveryMethod.Price = command.Price;
        deliveryMethod.CurrencyId = command.CurrencyId;
        deliveryMethod.EstimatedDays = command.EstimatedDays;
        deliveryMethod.Description = command.Description;
        deliveryMethod.ConstraintsJson = command.ConstraintsJson;
        deliveryMethod.DigitalContentType = command.DigitalContentType;
        deliveryMethod.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("روش ارسال با موفقیت ویرایش شد");
    }
}
