using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAggregate;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Commands
{
    public class GuaranteeCreateCommand
    {
        public required string Title { get; set; }
        public long? CreateBy { get; set; }
    }

    public class GuaranteeCreateHandler
    {
        private readonly AppDbContext _context;

        public GuaranteeCreateHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<long>> Handle(GuaranteeCreateCommand command, CancellationToken cancellationToken)
        {
            var guarantee = new Guarantee
            {
                Title = command.Title,
                IsDelete = false,
                IsActive = true,
                IsUpdate = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = command.CreateBy
            };

            await _context.Guarantees.AddAsync(guarantee, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation<long>.ToSuccessResult("ضمان با موفقیت ایجاد شد.", guarantee.Id);
        }
    }
}
