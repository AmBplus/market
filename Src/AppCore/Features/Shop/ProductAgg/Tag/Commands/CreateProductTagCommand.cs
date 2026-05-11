using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace AppCore.Features.Shop.ProductAgg.Tag.Commands;
public class CreateProductTagCommand
{
    public required string Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public long? CreatedBy { get; set; }
}
public class CreateProductTagHandler
{
    private readonly AppDbContext _context;
    public CreateProductTagHandler(ILogger<CreateProductTagHandler> logger,AppDbContext context)
    {
        Logger = logger;
        _context = context;
    }
    public ILogger<CreateProductTagHandler> Logger { get; }
    public async Task<ResultOperation<long>> Handle(CreateProductTagCommand command)
    {
        var isDuplicate = await _context.ProductTags
            .AnyAsync(t => t.Name == command.Name && !t.IsDelete);
        if (isDuplicate)
            return ResultOperation<long>.ToFailedResult("تگی با این نام قبلاً ثبت شده است.");
        if (!string.IsNullOrWhiteSpace(command.Slug))
        {
            var isSlugDuplicate = await _context.ProductTags
                .AnyAsync(t => t.Slug == command.Slug && !t.IsDelete);
            if (isSlugDuplicate)
                return ResultOperation<long>.ToFailedResult("این Slug قبلاً استفاده شده است.");
        }
        var tag = new AppCore.Domains.Entities.Shop.ProductAgg.ProductTag
        {
            Name = command.Name,
            Slug = command.Slug,
            Description = command.Description,
            IsActive = command.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = command.CreatedBy,
            UpdatedBy = command.CreatedBy
        };
        await _context.ProductTags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return ResultOperation<long>.ToSuccessResult("تگ با موفقیت ایجاد شد.", tag.Id);
    }
}

