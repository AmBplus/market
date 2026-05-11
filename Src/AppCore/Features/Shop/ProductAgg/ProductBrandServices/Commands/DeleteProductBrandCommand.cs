using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.ProductBrandServices.Commands
{
    public class DeleteProductBrandCommand
    {
        public required long Id { get; set; }
        public required long UserId { get; set; }
    }

    public class DeleteProductBrandCommandHandler
    {
        private readonly AppDbContext _context;

        public DeleteProductBrandCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(DeleteProductBrandCommand command, CancellationToken cancellationToken)
        {
            var brand = await _context.ProductBrands.FindAsync(command.Id);

            if (brand == null)
            {
                return ResultOperation.ToFailedResult("Product brand not found");
            }

            brand.IsDelete = true;
            brand.IsActive = false;
            brand.UpdatedAt = DateTime.UtcNow;
            brand.UpdatedBy = command.UserId;
            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult();
        }
    }
}
