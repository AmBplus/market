using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.Sliders.Commands;

public class DeleteSliderCommand
{
    public long Id { get; set; }
}

public class DeleteSliderHandler
{
    private readonly AppDbContext _context;
    public DeleteSliderHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteSliderCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه اسلایدر نامعتبر است");

        var slider = await _context.Sliders.FindAsync(new object[] { command.Id }, ct);
        if (slider is null)
            return ResultOperation.ToFailedResult("اسلایدر یافت نشد");

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("اسلایدر با موفقیت حذف شد");
    }
}
