using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Settings;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Settings.EnumValues.Commands;

public class CreateEnumValueCommand
{
    public string EnumType { get; set; } = string.Empty;
    public string EnumKey { get; set; } = string.Empty;
    public int EnumId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateEnumValueHandler
{
    private readonly ECommerceDbContext _context;
    public CreateEnumValueHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateEnumValueCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.EnumType))
            return ResultOperation.ToFailedResult("نوع	enum الزامی است");

        if (string.IsNullOrWhiteSpace(command.EnumKey))
            return ResultOperation.ToFailedResult("کلید enum الزامی است");

        if (command.EnumId <= 0)
            return ResultOperation.ToFailedResult("شناسه enum نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام الزامی است");

        var exists = await _context.EnumValues.AnyAsync(
            e => e.EnumType == command.EnumType && e.EnumId == command.EnumId, ct);
        if (exists)
            return ResultOperation.ToFailedResult("این مقدار enum قبلاً ثبت شده است");

        var enumValue = new EnumValue
        {
            EnumType = command.EnumType,
            EnumKey = command.EnumKey,
            EnumId = command.EnumId,
            Name = command.Name,
            Description = command.Description,
            DisplayOrder = command.DisplayOrder,
            IsActive = command.IsActive
        };

        _context.Add(enumValue);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("مقدار enum با موفقیت ایجاد شد");
    }
}
