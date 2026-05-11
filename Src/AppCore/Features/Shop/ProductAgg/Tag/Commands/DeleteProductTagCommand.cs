// AppCore/Features/Shop/ProductAgg/Tag/Commands/DeleteTagCommand.cs
using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.Tag.Commands;

public class DeleteTagCommand
{
    public long Id { get; set; }
    public long? DeletedBy { get; set; }
}

public class DeleteTagHandler
{
    private readonly AppDbContext _context;

    public DeleteTagHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation> Handle(DeleteTagCommand command)
    {
        var tag = await _context.ProductTags
            .FirstOrDefaultAsync(t => t.Id == command.Id && !t.IsDelete);

        if (tag is null)
            return ResultOperation.ToFailedResult("تگ مورد نظر یافت نشد.");

        tag.IsDelete = true;
        tag.UpdatedAt = DateTime.UtcNow;
        tag.UpdatedBy = command.DeletedBy;

        await _context.SaveChangesAsync();

        return ResultOperation.ToSuccessResult("تگ با موفقیت حذف شد.");
    }
}
