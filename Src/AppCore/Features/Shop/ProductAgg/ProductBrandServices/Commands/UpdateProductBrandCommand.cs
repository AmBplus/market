using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.ProductBrandServices.Commands
{
    public class UpdateProductBrandCommand
    {
        public required long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public long? UserId { get; set; }
    }

    public class UpdateProductBrandCommandHandler
    {
        private readonly AppDbContext _context;

        public UpdateProductBrandCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(UpdateProductBrandCommand command, CancellationToken cancellationToken)
        {
            var brand = await _context.ProductBrands.FindAsync(command.Id);

            if (brand == null)
            {
                return ResultOperation.ToFailedResult("Product brand not found");
            }

            brand.Name = command.Name ?? brand.Name;
            brand.Slug = string.IsNullOrWhiteSpace(command.Slug) ? brand.Slug : command.Slug;
            brand.Description = command.Description ?? brand.Description;
            brand.LogoUrl = command.LogoUrl ?? brand.LogoUrl;
            brand.WebsiteUrl = command.WebsiteUrl ?? brand.WebsiteUrl;
            brand.UpdatedBy = command.UserId ;
            brand.UpdatedAt = DateTime.UtcNow;
            brand.IsActive = false;


            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult();
        }
    }
}
