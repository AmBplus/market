using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.Sliders.Commands;

public class UpdateSliderCommand
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SubTitle { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class UpdateSliderHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateSliderHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateSliderCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه اسلایدر نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Title))
            return ResultOperation.ToFailedResult("عنوان اسلایدر الزامی است");

        if (string.IsNullOrWhiteSpace(command.ImageUrl))
            return ResultOperation.ToFailedResult("تصویر اسلایدر الزامی است");

        var slider = await _context.Sliders.FindAsync(new object[] { command.Id }, ct);
        if (slider is null)
            return ResultOperation.ToFailedResult("اسلایدر یافت نشد");

        if (command.EndedAt.HasValue && command.StartedAt.HasValue && command.EndedAt.Value <= command.StartedAt.Value)
            return ResultOperation.ToFailedResult("تاریخ پایان باید بعد از تاریخ شروع باشد");

        slider.Title = command.Title;
        slider.SubTitle = command.SubTitle;
        slider.ImageUrl = command.ImageUrl;
        slider.MobileImageUrl = command.MobileImageUrl;
        slider.LinkUrl = command.LinkUrl;
        slider.LinkText = command.LinkText;
        slider.DisplayOrder = command.DisplayOrder;
        slider.IsActive = command.IsActive;
        slider.StartedAt = command.StartedAt;
        slider.EndedAt = command.EndedAt;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("اسلایدر با موفقیت ویرایش شد");
    }
}
