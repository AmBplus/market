using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Framework.TextHelper;

namespace AppCore.Features.Shop.ProductAgg.ProductBrandServices.Commands
{
    public class CreateProductBrandCommand
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
    }

    public class CreateProductBrandCommandHandler
    {
        private readonly AppDbContext _context;

        public CreateProductBrandCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(CreateProductBrandCommand command, CancellationToken cancellationToken)
        {
            var brand = new Domains.Entities.Shop.ProductAgg.ProductBrand
            {
                Name = command.Name,
                Slug = string.IsNullOrWhiteSpace(command.Slug) ? TextFixer.Slugify(command.Name) : command.Slug,
                Description = command.Description,
                LogoUrl = command.LogoUrl,
                WebsiteUrl = command.WebsiteUrl,
                IsActive = true,
                IsDelete = false
            };

            _context.ProductBrands.Add(brand);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult();
        }
    }
}
