using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.Widgets.Commands;

public class UpdateWidgetCommand
{
    public long Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class UpdateWidgetHandler
{
    private readonly AppDbContext _context;
    public UpdateWidgetHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateWidgetCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه ویجت نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Type))
            return ResultOperation.ToFailedResult("نوع ویجت الزامی است");

        if (string.IsNullOrWhiteSpace(command.Title))
            return ResultOperation.ToFailedResult("عنوان ویجت الزامی است");

        if (string.IsNullOrWhiteSpace(command.Position))
            return ResultOperation.ToFailedResult("موقعیت ویجت الزامی است");

        var widget = await _context.Widgets.FindAsync(new object[] { command.Id }, ct);
        if (widget is null)
            return ResultOperation.ToFailedResult("ویجت یافت نشد");

        if (command.EndedAt.HasValue && command.StartedAt.HasValue && command.EndedAt.Value <= command.StartedAt.Value)
            return ResultOperation.ToFailedResult("تاریخ پایان باید بعد از تاریخ شروع باشد");

        widget.Type = command.Type;
        widget.Title = command.Title;
        widget.ConfigJson = command.ConfigJson;
        widget.Position = command.Position;
        widget.DisplayOrder = command.DisplayOrder;
        widget.IsActive = command.IsActive;
        widget.StartedAt = command.StartedAt;
        widget.EndedAt = command.EndedAt;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("ویجت با موفقیت ویرایش شد");
    }
}
