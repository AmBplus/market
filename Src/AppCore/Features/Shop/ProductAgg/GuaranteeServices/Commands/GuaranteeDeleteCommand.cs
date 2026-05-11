using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Commands;

public record GuaranteeDeleteCommand(long Id, long? DeletedBy);

public class GuaranteeDeleteHandler
{
    private readonly AppDbContext _context;

    public GuaranteeDeleteHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation> Handle(GuaranteeDeleteCommand command, CancellationToken cancellationToken)
    {
        var guarantee = await _context.Guarantees.FindAsync(new object[] { command.Id }, cancellationToken);

        if (guarantee is null)
            return ResultOperation.ToFailedResult("ضمان مورد نظر پیدا نشد.");

        guarantee.IsDeleted = true;
        guarantee.IsActive = false;
        guarantee.UpdatedBy = command.DeletedBy;

        await _context.SaveChangesAsync(cancellationToken);

        return ResultOperation.ToSuccessResult("ضمان با موفقیت حذف شد.");
    }
}
