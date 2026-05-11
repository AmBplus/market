using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Payment;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Payment.PaymentGateways.Commands;

public class CreatePaymentGatewayCommand
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreatePaymentGatewayHandler
{
    private readonly ECommerceDbContext _context;
    public CreatePaymentGatewayHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreatePaymentGatewayCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام درگاه الزامی است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد درگاه الزامی است");

        var codeExists = await _context.PaymentGateways.AnyAsync(g => g.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد درگاه تکراری است");

        var gateway = new PaymentGateway
        {
            Name = command.Name,
            Code = command.Code,
            ConfigJson = command.ConfigJson,
            Priority = command.Priority,
            IsActive = command.IsActive
        };

        _context.Add(gateway);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("درگاه پرداخت با موفقیت ایجاد شد");
    }
}
