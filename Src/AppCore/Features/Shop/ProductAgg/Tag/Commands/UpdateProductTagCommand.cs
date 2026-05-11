using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.Tag.Commands;

public class UpdateTagCommand
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public string? Slug { get; set; }
    public bool IsActive { get; set; }
    public long? UpdatedBy { get; set; }
}

public class UpdateTagHandler
{
    private readonly AppDbContext _context;

    public UpdateTagHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation> Handle(UpdateTagCommand command)
    {
        var tag = await _context.ProductTags
            .FirstOrDefaultAsync(t => t.Id == command.Id && !t.IsDeleted);

        if (tag is null)
            return ResultOperation.ToFailedResult("تگ مورد نظر یافت نشد.");

        var isDuplicate = await _context.ProductTags
            .AnyAsync(t => t.Title == command.Title && t.Id != command.Id && !t.IsDeleted);

        if (isDuplicate)
            return ResultOperation.ToFailedResult("تگی با این نام قبلاً ثبت شده است.");

        if (!string.IsNullOrWhiteSpace(command.Slug))
        {
            var isSlugDuplicate = await _context.ProductTags
                .AnyAsync(t => t.Slug == command.Slug && t.Id != command.Id && !t.IsDeleted);

            if (isSlugDuplicate)
                return ResultOperation.ToFailedResult("این Slug قبلاً استفاده شده است.");
        }

        tag.Title = command.Title;
        tag.Slug = command.Slug;
        tag.IsActive = command.IsActive;
        tag.UpdatedBy = command.UpdatedBy;

        await _context.SaveChangesAsync();

        return ResultOperation.ToSuccessResult("تگ با موفقیت ویرایش شد.");
    }
}
