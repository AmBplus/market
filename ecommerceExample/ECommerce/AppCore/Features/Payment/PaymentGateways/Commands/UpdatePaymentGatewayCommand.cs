using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.PaymentGateways.Commands;

public class UpdatePaymentGatewayCommand
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}

public class UpdatePaymentGatewayHandler
{
    private readonly ECommerceDbContext _context;
    public UpdatePaymentGatewayHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdatePaymentGatewayCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه درگاه نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام درگاه الزامی است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد درگاه الزامی است");

        var gateway = await _context.PaymentGateways.FindAsync(new object[] { command.Id }, ct);
        if (gateway is null)
            return ResultOperation.ToFailedResult("درگاه پرداخت یافت نشد");

        var codeExists = await _context.PaymentGateways.AnyAsync(g => g.Code == command.Code && g.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد درگاه تکراری است");

        gateway.Name = command.Name;
        gateway.Code = command.Code;
        gateway.ConfigJson = command.ConfigJson;
        gateway.Priority = command.Priority;
        gateway.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("درگاه پرداخت با موفقیت ویرایش شد");
    }
}
