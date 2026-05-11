using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.Widgets.Commands;

public class DeleteWidgetCommand
{
    public long Id { get; set; }
}

public class DeleteWidgetHandler
{
    private readonly ECommerceDbContext _context;
    public DeleteWidgetHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteWidgetCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه ویجت نامعتبر است");

        var widget = await _context.Widgets.FindAsync(new object[] { command.Id }, ct);
        if (widget is null)
            return ResultOperation.ToFailedResult("ویجت یافت نشد");

        _context.Widgets.Remove(widget);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("ویجت با موفقیت حذف شد");
    }
}
