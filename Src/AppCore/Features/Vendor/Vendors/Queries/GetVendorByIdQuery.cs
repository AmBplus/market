using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Queries;

public class GetVendorByIdQuery
{
    public long Id { get; set; }
}

public class VendorDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string StoreSlug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public decimal CommissionRate { get; set; }
    public VendorStatus Status { get; set; }
    public string? BankAccountNumber { get; set; }
    public bool IsActive { get; set; }
}

public class GetVendorByIdHandler
{
    private readonly AppDbContext _context;
    public GetVendorByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<VendorDto>> Handle(GetVendorByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<VendorDto>.ToFailedResult("شناسه فروشنده نامعتبر است");

        var vendor = await _context.Vendors
            .AsNoTracking()
            .Where(v => v.Id == query.Id)
            .Select(v => new VendorDto
            {
                Id = v.Id,
                UserId = v.UserId,
                UserName = v.User.UserName ?? "",
                StoreName = v.StoreName,
                StoreSlug = v.StoreSlug,
                Description = v.Description,
                LogoUrl = v.LogoUrl,
                CommissionRate = v.CommissionRate,
                Status = v.Status,
                BankAccountNumber = v.BankAccountNumber,
                IsActive = v.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (vendor is null)
            return ResultOperation<VendorDto>.ToFailedResult("فروشنده یافت نشد");

        return ResultOperation<VendorDto>.ToSuccessResult(vendor);
    }
}
