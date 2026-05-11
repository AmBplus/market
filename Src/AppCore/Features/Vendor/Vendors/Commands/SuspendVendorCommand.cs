using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Commands;

public class SuspendVendorCommand
{
    public long Id { get; set; }
}

public class SuspendVendorHandler
{
    private readonly AppDbContext _context;
    public SuspendVendorHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(SuspendVendorCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه فروشنده نامعتبر است");

        var vendor = await _context.Vendors.FindAsync(new object[] { command.Id }, ct);
        if (vendor is null)
            return ResultOperation.ToFailedResult("فروشنده یافت نشد");

        if (vendor.Status == VendorStatus.Suspended)
            return ResultOperation.ToFailedResult("فروشنده قبلاً تعلیق شده است");

        vendor.Status = VendorStatus.Suspended;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("فروشنده با موفقیت تعلیق شد");
    }
}
