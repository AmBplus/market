using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Customization;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.Sliders.Commands;

public class CreateSliderCommand
{
    public string Title { get; set; } = string.Empty;
    public string? SubTitle { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class CreateSliderHandler
{
    private readonly ECommerceDbContext _context;
    public CreateSliderHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateSliderCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            return ResultOperation.ToFailedResult("عنوان اسلایدر الزامی است");

        if (string.IsNullOrWhiteSpace(command.ImageUrl))
            return ResultOperation.ToFailedResult("تصویر اسلایدر الزامی است");

        if (command.EndedAt.HasValue && command.StartedAt.HasValue && command.EndedAt.Value <= command.StartedAt.Value)
            return ResultOperation.ToFailedResult("تاریخ پایان باید بعد از تاریخ شروع باشد");

        var slider = new Slider
        {
            Title = command.Title,
            SubTitle = command.SubTitle,
            ImageUrl = command.ImageUrl,
            MobileImageUrl = command.MobileImageUrl,
            LinkUrl = command.LinkUrl,
            LinkText = command.LinkText,
            DisplayOrder = command.DisplayOrder,
            IsActive = command.IsActive,
            StartedAt = command.StartedAt,
            EndedAt = command.EndedAt
        };

        _context.Add(slider);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("اسلایدر با موفقیت ایجاد شد");
    }
}
