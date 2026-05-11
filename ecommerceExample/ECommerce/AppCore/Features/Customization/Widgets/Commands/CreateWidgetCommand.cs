using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Customization;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.Widgets.Commands;

public class CreateWidgetCommand
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class CreateWidgetHandler
{
    private readonly ECommerceDbContext _context;
    public CreateWidgetHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateWidgetCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Type))
            return ResultOperation.ToFailedResult("نوع ویجت الزامی است");

        if (string.IsNullOrWhiteSpace(command.Title))
            return ResultOperation.ToFailedResult("عنوان ویجت الزامی است");

        if (string.IsNullOrWhiteSpace(command.Position))
            return ResultOperation.ToFailedResult("موقعیت ویجت الزامی است");

        if (command.EndedAt.HasValue && command.StartedAt.HasValue && command.EndedAt.Value <= command.StartedAt.Value)
            return ResultOperation.ToFailedResult("تاریخ پایان باید بعد از تاریخ شروع باشد");

        var widget = new Widget
        {
            Type = command.Type,
            Title = command.Title,
            ConfigJson = command.ConfigJson,
            Position = command.Position,
            DisplayOrder = command.DisplayOrder,
            IsActive = command.IsActive,
            StartedAt = command.StartedAt,
            EndedAt = command.EndedAt
        };

        _context.Add(widget);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("ویجت با موفقیت ایجاد شد");
    }
}
