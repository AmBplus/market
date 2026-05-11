using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAggregate;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Commands
{
    public class GuaranteeUpdateCommand
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public long? UpdateBy { get; set; }
    }

    public class GuaranteeUpdateHandler
    {
        private readonly AppDbContext _context;

        public GuaranteeUpdateHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(GuaranteeUpdateCommand command, CancellationToken cancellationToken)
        {
            var guarantee = await _context.Guarantees
                .FirstOrDefaultAsync(t => t.Id == command.Id && !t.IsDelete, cancellationToken);

            if (guarantee is null)
                return ResultOperation.ToFailedResult("ضمان مورد نظر پیدا نشد.");

            guarantee.Title = command.Title;
            guarantee.IsActive = command.IsActive;
            guarantee.IsDelete = command.IsDelete;
            guarantee.UpdateAt = DateTime.UtcNow;
            guarantee.UpdatedAt = DateTime.UtcNow;
            guarantee.UpdateBy = command.UpdateBy;

            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult("ضمان با موفقیت ویرایش شد.");
        }
    }
}
