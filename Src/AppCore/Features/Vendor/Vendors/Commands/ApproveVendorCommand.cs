using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Commands;

public class ApproveVendorCommand
{
    public long Id { get; set; }
}

public class ApproveVendorHandler
{
    private readonly AppDbContext _context;
    public ApproveVendorHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(ApproveVendorCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه فروشنده نامعتبر است");

        var vendor = await _context.Vendors.FindAsync(new object[] { command.Id }, ct);
        if (vendor is null)
            return ResultOperation.ToFailedResult("فروشنده یافت نشد");

        if (vendor.Status == VendorStatus.Active)
            return ResultOperation.ToFailedResult("فروشنده قبلاً تایید شده است");

        vendor.Status = VendorStatus.Active;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("فروشنده با موفقیت تایید شد");
    }
}
