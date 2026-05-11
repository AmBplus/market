using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Shipping;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.DeliveryMethods.Commands;

public class CreateDeliveryMethodCommand
{
    public string Name { get; set; } = string.Empty;
    public DeliveryType DeliveryType { get; set; }
    public long? CarrierId { get; set; }
    public decimal Price { get; set; }
    public long CurrencyId { get; set; }
    public int? EstimatedDays { get; set; }
    public string? Description { get; set; }
    public string? ConstraintsJson { get; set; }
    public DigitalContentType? DigitalContentType { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateDeliveryMethodHandler
{
    private readonly ECommerceDbContext _context;
    public CreateDeliveryMethodHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateDeliveryMethodCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام روش ارسال الزامی است");

        if (command.Price < 0)
            return ResultOperation.ToFailedResult("قیمت ارسال نمی‌تواند منفی باشد");

        if (command.CarrierId.HasValue)
        {
            var carrierExists = await _context.Carriers.AnyAsync(c => c.Id == command.CarrierId.Value && c.IsActive, ct);
            if (!carrierExists)
                return ResultOperation.ToFailedResult("شرکت حمل یافت نشد یا غیرفعال است");
        }

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == command.CurrencyId, ct);
        if (!currencyExists)
            return ResultOperation.ToFailedResult("واحد پول یافت نشد");

        var deliveryMethod = new DeliveryMethod
        {
            Name = command.Name,
            DeliveryType = command.DeliveryType,
            CarrierId = command.CarrierId,
            Price = command.Price,
            CurrencyId = command.CurrencyId,
            EstimatedDays = command.EstimatedDays,
            Description = command.Description,
            ConstraintsJson = command.ConstraintsJson,
            DigitalContentType = command.DigitalContentType,
            IsActive = command.IsActive
        };

        _context.Add(deliveryMethod);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("روش ارسال با موفقیت ایجاد شد");
    }
}
