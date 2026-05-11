using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Product;
using ECommerce.AppCore.Features.Product.Products.Commands;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace Tests.Features.Product;

public class ProductCommandTests
{
    private async Task<ECommerceDbContext> GetDbContextWithSeedAsync()
    {
        var context = TestHelper.GetInMemoryDbContext();

        context.Brands.Add(new Brand
        {
            Name = "سامسونگ",
            Slug = "samsung",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        context.Categories.Add(new Category
        {
            Code = "ELEC",
            Name = "الکترونیک",
            Slug = "electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task CreateProduct_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreateProductHandler(context);
        var command = new CreateProductCommand
        {
            ProductCode = "PROD-001",
            Name = "گوشی موبایل سامسونگ A54",
            Slug = "samsung-a54",
            Description = "گوشی موبایل سامسونگ مدل A54",
            ShortDescription = "گوشی میان‌رده سامسونگ",
            BrandId = 1,
            CategoryIds = new List<long> { 1 },
            Attributes = new List<CreateProductAttributeDto>
            {
                new() { Key = "رنگ", Value = "مشکی", DisplayOrder = 1 },
                new() { Key = "حافظه داخلی", Value = "128 گیگابایت", DisplayOrder = 2 }
            },
            IsActive = true,
            IsFeatured = true
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("محصول با موفقیت ایجاد شد", result.MessageSingle);

        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductCode == "PROD-001");
        Assert.NotNull(product);
        Assert.Equal("گوشی موبایل سامسونگ A54", product.Name);
        Assert.Equal(1, product.BrandId);
        Assert.True(product.IsFeatured);

        var productCategories = await context.ProductCategories.Where(pc => pc.ProductId == product.Id).ToListAsync();
        Assert.Single(productCategories);

        var productAttributes = await context.ProductAttributes.Where(pa => pa.ProductId == product.Id).ToListAsync();
        Assert.Equal(2, productAttributes.Count);
    }

    [Fact]
    public async Task CreateProduct_DuplicateCode_ReturnsFailedResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreateProductHandler(context);

        var firstCommand = new CreateProductCommand
        {
            ProductCode = "PROD-DUP",
            Name = "محصول اول",
            Slug = "product-first"
        };
        await handler.Handle(firstCommand, CancellationToken.None);

        var secondCommand = new CreateProductCommand
        {
            ProductCode = "PROD-DUP",
            Name = "محصول دوم",
            Slug = "product-second"
        };

        // Act
        var result = await handler.Handle(secondCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("کد محصول تکراری است", result.MessageSingle);
    }

    [Fact]
    public async Task UpdateProduct_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var createHandler = new CreateProductHandler(context);
        await createHandler.Handle(new CreateProductCommand
        {
            ProductCode = "PROD-UPD",
            Name = "نام قدیمی",
            Slug = "old-slug",
            BrandId = 1,
            CategoryIds = new List<long> { 1 },
            IsActive = true
        }, CancellationToken.None);

        var updateHandler = new UpdateProductHandler(context);
        var command = new UpdateProductCommand
        {
            Id = 1,
            ProductCode = "PROD-UPD",
            Name = "نام جدید محصول",
            Slug = "new-slug",
            BrandId = 1,
            CategoryIds = new List<long> { 1 },
            IsActive = true,
            IsFeatured = true
        };

        // Act
        var result = await updateHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("محصول با موفقیت ویرایش شد", result.MessageSingle);

        var product = await context.Products.FindAsync((long)1);
        Assert.NotNull(product);
        Assert.Equal("نام جدید محصول", product.Name);
        Assert.Equal("new-slug", product.Slug);
        Assert.True(product.IsFeatured);
    }

    [Fact]
    public async Task DeleteProduct_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = TestHelper.GetInMemoryDbContext();
        context.Products.Add(new Domain.Product.Product
        {
            ProductCode = "PROD-DEL",
            Name = "محصول حذفی",
            Slug = "delete-product",
            IsActive = true,
            IsFeatured = false,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var handler = new DeleteProductHandler(context);
        var command = new DeleteProductCommand { Id = 1 };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("محصول با موفقیت حذف شد", result.MessageSingle);

        // Refresh from DB - bypass query filter
        var product = await context.Products
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == 1);
        Assert.NotNull(product);
        Assert.True(product.IsDeleted);
    }

    [Fact]
    public async Task DeleteProduct_WithActiveVariants_ReturnsFailedResult()
    {
        // Arrange
        var context = TestHelper.GetInMemoryDbContext();
        var product = new Domain.Product.Product
        {
            ProductCode = "PROD-VAR",
            Name = "محصول با تنوع",
            Slug = "product-with-variant",
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        context.ProductVariants.Add(new ProductVariant
        {
            ProductId = product.Id,
            VariantCode = "VAR-001",
            Sku = "SKU-001",
            Title = "تنوع فعال",
            DefaultPrice = 1000000,
            CurrencyId = 1,
            IsActive = true,
            IsDeleted = false
        });
        await context.SaveChangesAsync();

        var handler = new DeleteProductHandler(context);
        var command = new DeleteProductCommand { Id = product.Id };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("این محصول دارای تنوع فعال است و قابل حذف نیست", result.MessageSingle);
    }
}
