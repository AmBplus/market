using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Shipping.Carriers.Commands;

public class UpdateCarrierCommand
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? TrackingUrlTemplate { get; set; }
    public string? ConfigJson { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCarrierHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateCarrierHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateCarrierCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه شرکت حمل نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام شرکت حمل الزامی است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد شرکت حمل الزامی است");

        var carrier = await _context.Carriers.FindAsync(new object[] { command.Id }, ct);
        if (carrier is null)
            return ResultOperation.ToFailedResult("شرکت حمل یافت نشد");

        var codeExists = await _context.Carriers.AnyAsync(c => c.Code == command.Code && c.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد شرکت حمل تکراری است");

        carrier.Name = command.Name;
        carrier.Code = command.Code;
        carrier.TrackingUrlTemplate = command.TrackingUrlTemplate;
        carrier.ConfigJson = command.ConfigJson;
        carrier.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("شرکت حمل با موفقیت ویرایش شد");
    }
}
