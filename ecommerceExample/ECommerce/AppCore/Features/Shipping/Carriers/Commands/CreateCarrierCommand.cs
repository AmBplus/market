using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Shipping;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.Carriers.Commands;

public class CreateCarrierCommand
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? TrackingUrlTemplate { get; set; }
    public string? ConfigJson { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateCarrierHandler
{
    private readonly ECommerceDbContext _context;
    public CreateCarrierHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateCarrierCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام شرکت حمل الزامی است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد شرکت حمل الزامی است");

        var codeExists = await _context.Carriers.AnyAsync(c => c.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد شرکت حمل تکراری است");

        var carrier = new Carrier
        {
            Name = command.Name,
            Code = command.Code,
            TrackingUrlTemplate = command.TrackingUrlTemplate,
            ConfigJson = command.ConfigJson,
            IsActive = command.IsActive
        };

        _context.Add(carrier);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("شرکت حمل با موفقیت ایجاد شد");
    }
}
