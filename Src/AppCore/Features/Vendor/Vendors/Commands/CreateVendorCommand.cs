using AppCore.Data;
using AppCore.Domains.Vendor;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Commands;

public class CreateVendorCommand
{
    public long UserId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreSlug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public decimal CommissionRate { get; set; }
    public string? BankAccountNumber { get; set; }
}

public class CreateVendorHandler
{
    private readonly AppDbContext _context;
    public CreateVendorHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateVendorCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.StoreName))
            return ResultOperation.ToFailedResult("نام فروشگاه الزامی است");

        if (string.IsNullOrWhiteSpace(command.StoreSlug))
            return ResultOperation.ToFailedResult("اسلاگ فروشگاه الزامی است");

        if (command.CommissionRate < 0 || command.CommissionRate > 100)
            return ResultOperation.ToFailedResult("نرخ پورسانت باید بین ۰ تا ۱۰۰ باشد");

        var userExists = await _context.Users.AnyAsync(u => u.Id == command.UserId, ct);
        if (!userExists)
            return ResultOperation.ToFailedResult("کاربر یافت نشد");

        var slugExists = await _context.Vendors.AnyAsync(v => v.StoreSlug == command.StoreSlug, ct);
        if (slugExists)
            return ResultOperation.ToFailedResult("اسلاگ فروشگاه تکراری است");

        var userHasVendor = await _context.Vendors.AnyAsync(v => v.UserId == command.UserId, ct);
        if (userHasVendor)
            return ResultOperation.ToFailedResult("این کاربر قبلاً فروشگاه دارد");

        var vendor = new Domains.Vendor.Vendor
        {
            UserId = command.UserId,
            StoreName = command.StoreName,
            StoreSlug = command.StoreSlug,
            Description = command.Description,
            LogoUrl = command.LogoUrl,
            CommissionRate = command.CommissionRate,
            Status = VendorStatus.Pending,
            BankAccountNumber = command.BankAccountNumber,
            IsActive = true
        };

        _context.Add(vendor);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("فروشنده با موفقیت ایجاد شد");
    }
}
