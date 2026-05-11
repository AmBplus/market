using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Commands;

public class UpdateVendorCommand
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreSlug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public decimal CommissionRate { get; set; }
    public string? BankAccountNumber { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateVendorHandler
{
    private readonly AppDbContext _context;
    public UpdateVendorHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateVendorCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه فروشنده نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.StoreName))
            return ResultOperation.ToFailedResult("نام فروشگاه الزامی است");

        if (string.IsNullOrWhiteSpace(command.StoreSlug))
            return ResultOperation.ToFailedResult("اسلاگ فروشگاه الزامی است");

        if (command.CommissionRate < 0 || command.CommissionRate > 100)
            return ResultOperation.ToFailedResult("نرخ پورسانت باید بین ۰ تا ۱۰۰ باشد");

        var vendor = await _context.Vendors.FindAsync(new object[] { command.Id }, ct);
        if (vendor is null)
            return ResultOperation.ToFailedResult("فروشنده یافت نشد");

        var slugExists = await _context.Vendors.AnyAsync(v => v.StoreSlug == command.StoreSlug && v.Id != command.Id, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ فروشگاه تکراری است");

        vendor.StoreName = command.StoreName;
        vendor.StoreSlug = command.StoreSlug;
        vendor.Description = command.Description;
        vendor.LogoUrl = command.LogoUrl;
        vendor.CommissionRate = command.CommissionRate;
        vendor.BankAccountNumber = command.BankAccountNumber;
        vendor.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("فروشنده با موفقیت ویرایش شد");
    }
}
