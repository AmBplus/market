using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Pricing.Currencies.Commands;

public class UpdateCurrencyCommand
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public bool IsBase { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCurrencyHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateCurrencyHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateCurrencyCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه واحد پول نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد واحد پول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام واحد پول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Symbol))
            return ResultOperation.ToFailedResult("نماد واحد پول الزامی است");

        if (command.ExchangeRate <= 0)
            return ResultOperation.ToFailedResult("نرخ تبدیل باید بزرگتر از صفر باشد");

        var currency = await _context.Currencies.FindAsync(new object[] { command.Id }, ct);
        if (currency is null)
            return ResultOperation.ToFailedResult("واحد پول یافت نشد");

        var codeExists = await _context.Currencies.AnyAsync(c => c.Code == command.Code && c.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد واحد پول تکراری است");

        if (command.IsBase && !currency.IsBase)
        {
            var existingBase = await _context.Currencies.AnyAsync(c => c.IsBase && c.Id != command.Id, ct);
            if (existingBase)
                return ResultOperation.ToFailedResult("یک واحد پول پایه از قبل وجود دارد");
        }

        currency.Code = command.Code;
        currency.Name = command.Name;
        currency.Symbol = command.Symbol;
        currency.ExchangeRate = command.ExchangeRate;
        currency.IsBase = command.IsBase;
        currency.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("واحد پول با موفقیت ویرایش شد");
    }
}
