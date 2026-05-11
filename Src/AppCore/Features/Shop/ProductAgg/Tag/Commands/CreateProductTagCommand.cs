using AppCore.Data;
using AppCore.Domains.Shop;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.Tag.Commands;

public class CreateProductTagCommand
{
    public required string Title { get; set; }
    public string? Slug { get; set; }
    public bool IsActive { get; set; } = true;
    public long? CreatedBy { get; set; }
}

public class CreateProductTagHandler
{
    private readonly AppDbContext _context;

    public CreateProductTagHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<long>> Handle(CreateProductTagCommand command)
    {
        var isDuplicate = await _context.ProductTags
            .AnyAsync(t => t.Title == command.Title && !t.IsDeleted);
        if (isDuplicate)
            return ResultOperation<long>.ToFailedResult("تگی با این نام قبلاً ثبت شده است.");

        if (!string.IsNullOrWhiteSpace(command.Slug))
        {
            var isSlugDuplicate = await _context.ProductTags
                .AnyAsync(t => t.Slug == command.Slug && !t.IsDeleted);
            if (isSlugDuplicate)
                return ResultOperation<long>.ToFailedResult("این Slug قبلاً استفاده شده است.");
        }

        var tag = new ProductTag
        {
            Title = command.Title,
            Slug = command.Slug,
            IsActive = command.IsActive,
            CreatedBy = command.CreatedBy
        };

        await _context.ProductTags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return ResultOperation<long>.ToSuccessResult("تگ با موفقیت ایجاد شد.", tag.Id);
    }
}
