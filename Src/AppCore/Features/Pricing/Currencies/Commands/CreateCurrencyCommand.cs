using AppCore.Data;
using AppCore.Domains.Pricing;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Pricing.Currencies.Commands;

public class CreateCurrencyCommand
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public bool IsBase { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateCurrencyHandler
{
    private readonly AppDbContext _context;
    public CreateCurrencyHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateCurrencyCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد واحد پول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام واحد پول الزامی است");

        if (string.IsNullOrWhiteSpace(command.Symbol))
            return ResultOperation.ToFailedResult("نماد واحد پول الزامی است");

        if (command.ExchangeRate <= 0)
            return ResultOperation.ToFailedResult("نرخ تبدیل باید بزرگتر از صفر باشد");

        var codeExists = await _context.Currencies.AnyAsync(c => c.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد واحد پول تکراری است");

        if (command.IsBase)
        {
            var existingBase = await _context.Currencies.AnyAsync(c => c.IsBase, ct);
            if (existingBase)
                return ResultOperation.ToFailedResult("یک واحد پول پایه از قبل وجود دارد");
        }

        var currency = new Currency
        {
            Code = command.Code,
            Name = command.Name,
            Symbol = command.Symbol,
            ExchangeRate = command.ExchangeRate,
            IsBase = command.IsBase,
            IsActive = command.IsActive
        };

        _context.Add(currency);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("واحد پول با موفقیت ایجاد شد");
    }
}
