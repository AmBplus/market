using System;
using System.Collections.Generic;
using System.Text;

// AppCore/Features/Shop/Tags/Commands/UpdateTag/UpdateTagCommand.cs
using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;


namespace AppCore.Features.Shop.ProductAgg.Tag.Commands;



public class UpdateTagCommand
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
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
            .FirstOrDefaultAsync(t => t.Id == command.Id && !t.IsDelete);

        if (tag is null)
            return ResultOperation.ToFailedResult("تگ مورد نظر یافت نشد.");

        // بررسی تکراری نبودن نام (به جز خودش)
        var isDuplicate = await _context.ProductTags
            .AnyAsync(t => t.Name == command.Name
                        && t.Id != command.Id
                        && !t.IsDelete);

        if (isDuplicate)
            return ResultOperation.ToFailedResult("تگی با این نام قبلاً ثبت شده است.");

        // بررسی تکراری نبودن slug (به جز خودش)
        if (!string.IsNullOrWhiteSpace(command.Slug))
        {
            var isSlugDuplicate = await _context.ProductTags
                .AnyAsync(t => t.Slug == command.Slug
                            && t.Id != command.Id
                            && !t.IsDelete);

            if (isSlugDuplicate)
                return ResultOperation.ToFailedResult("این Slug قبلاً استفاده شده است.");
        }

        tag.Name = command.Name;
        tag.Slug = command.Slug;
        tag.Description = command.Description;
        tag.IsActive = command.IsActive;
        tag.UpdatedAt = DateTime.UtcNow;
        tag.UpdatedBy = command.UpdatedBy;

        await _context.SaveChangesAsync();

        return ResultOperation.ToSuccessResult("تگ با موفقیت ویرایش شد.");
    }
}

