using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAggregate;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Commands
{
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
            var guarantee = await _context.Guarantees.FindAsync(command.Id, cancellationToken);

            if (guarantee is null)
                return ResultOperation.ToFailedResult("ضمان مورد نظر پیدا نشد.");

            guarantee.IsDelete = true;
            guarantee.IsActive = false;
            guarantee.UpdateAt = DateTime.UtcNow;
            guarantee.UpdatedAt = DateTime.UtcNow;
            guarantee.UpdateBy = command.DeletedBy;

            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult("ضمان با موفقیت حذف شد.");
        }
    }
}
