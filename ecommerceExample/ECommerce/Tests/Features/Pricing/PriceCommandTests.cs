using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Pricing;
using ECommerce.AppCore.Domain.Product;
using ECommerce.AppCore.Enums;
using ECommerce.AppCore.Features.Pricing.Prices.Commands;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace Tests.Features.Pricing;

public class PriceCommandTests
{
    private async Task<ECommerceDbContext> GetDbContextWithSeedAsync()
    {
        var context = TestHelper.GetInMemoryDbContext();

        // Seed currency
        context.Currencies.AddRange(
            new Currency
            {
                Code = "IRR",
                Name = "تومان",
                Symbol = "ت",
                ExchangeRate = 1.0m,
                IsBase = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Currency
            {
                Code = "USD",
                Name = "دلار آمریکا",
                Symbol = "$",
                ExchangeRate = 50000m,
                IsBase = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );

        await context.SaveChangesAsync();

        // Seed product + variant
        var product = new Domain.Product.Product
        {
            ProductCode = "PRC-PROD-001",
            Name = "لپ‌تاپ ایسوس",
            Slug = "asus-laptop",
            IsActive = true,
            IsDeleted = false,
            IsFeatured = false,
            CreatedAt = DateTime.UtcNow
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        context.ProductVariants.Add(new ProductVariant
        {
            ProductId = product.Id,
            VariantCode = "PRC-VAR-001",
            Sku = "PRC-SKU-001",
            Title = "i7 / 16GB / 512GB",
            DefaultPrice = 35000000,
            CurrencyId = 1,
            IsActive = true,
            IsDeleted = false
        });
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task CreatePrice_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreatePriceHandler(context);
        var command = new CreatePriceCommand
        {
            ProductVariantId = 1,
            Amount = 35000000m,
            CurrencyId = 1,
            PriceType = PriceType.Retail,
            StartedAt = DateTime.UtcNow
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("قیمت با موفقیت ایجاد شد", result.MessageSingle);

        var price = await context.Prices.FirstOrDefaultAsync(p => p.ProductVariantId == 1);
        Assert.NotNull(price);
        Assert.Equal(35000000m, price.Amount);
        Assert.Equal(PriceType.Retail, price.PriceType);
        Assert.Null(price.EndedAt); // Active price
    }

    [Fact]
    public async Task CreatePrice_ClosesPreviousPrice_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreatePriceHandler(context);

        // Create first price
        await handler.Handle(new CreatePriceCommand
        {
            ProductVariantId = 1,
            Amount = 30000000m,
            CurrencyId = 1,
            PriceType = PriceType.Retail,
            StartedAt = DateTime.UtcNow.AddDays(-10)
        }, CancellationToken.None);

        // Create second price for same variant + type
        var command = new CreatePriceCommand
        {
            ProductVariantId = 1,
            Amount = 32000000m,
            CurrencyId = 1,
            PriceType = PriceType.Retail,
            StartedAt = DateTime.UtcNow
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        var prices = await context.Prices
            .Where(p => p.ProductVariantId == 1 && p.PriceType == PriceType.Retail)
            .OrderBy(p => p.StartedAt)
            .ToListAsync();

        Assert.Equal(2, prices.Count);

        // First price should be closed
        Assert.NotNull(prices[0].EndedAt);

        // Second price should be active
        Assert.Null(prices[1].EndedAt);
        Assert.Equal(32000000m, prices[1].Amount);
    }

    [Fact]
    public async Task CreatePrice_DifferentPriceType_DoesNotClosePrevious()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreatePriceHandler(context);

        // Create Retail price
        await handler.Handle(new CreatePriceCommand
        {
            ProductVariantId = 1,
            Amount = 35000000m,
            CurrencyId = 1,
            PriceType = PriceType.Retail,
            StartedAt = DateTime.UtcNow
        }, CancellationToken.None);

        // Create Wholesale price (different type)
        await handler.Handle(new CreatePriceCommand
        {
            ProductVariantId = 1,
            Amount = 30000000m,
            CurrencyId = 1,
            PriceType = PriceType.Wholesale,
            StartedAt = DateTime.UtcNow
        }, CancellationToken.None);

        // Assert - Retail price should still be active
        var retailPrice = await context.Prices
            .FirstOrDefaultAsync(p => p.ProductVariantId == 1 && p.PriceType == PriceType.Retail);
        Assert.NotNull(retailPrice);
        Assert.Null(retailPrice.EndedAt); // Still active

        var wholesalePrice = await context.Prices
            .FirstOrDefaultAsync(p => p.ProductVariantId == 1 && p.PriceType == PriceType.Wholesale);
        Assert.NotNull(wholesalePrice);
        Assert.Null(wholesalePrice.EndedAt); // Active
        Assert.Equal(30000000m, wholesalePrice.Amount);
    }

    [Fact]
    public async Task BulkPriceUpdate_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var createHandler = new CreatePriceHandler(context);

        // Create initial retail price
        await createHandler.Handle(new CreatePriceCommand
        {
            ProductVariantId = 1,
            Amount = 10000000m,
            CurrencyId = 1,
            PriceType = PriceType.Retail,
            StartedAt = DateTime.UtcNow
        }, CancellationToken.None);

        var bulkHandler = new BulkPriceUpdateHandler(context);
        var command = new BulkPriceUpdateCommand
        {
            ProductIds = new List<long> { 1 },
            PercentageIncrease = 10m, // 10% increase
            PriceType = PriceType.Retail,
            CurrencyId = 1
        };

        // Act
        var result = await bulkHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains("با موفقیت بروزرسانی شد", result.MessageSingle);

        // Old price should be closed
        var oldPrice = await context.Prices
            .FirstOrDefaultAsync(p => p.Amount == 10000000m && p.PriceType == PriceType.Retail);
        Assert.NotNull(oldPrice);
        Assert.NotNull(oldPrice.EndedAt);

        // New price should be active with increased amount
        var newPrice = await context.Prices
            .FirstOrDefaultAsync(p => p.EndedAt == null && p.PriceType == PriceType.Retail);
        Assert.NotNull(newPrice);
        Assert.Equal(11000000m, newPrice.Amount); // 10000000 + 10%
    }

    [Fact]
    public async Task BulkPriceUpdate_ZeroPercentage_ReturnsFailedResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new BulkPriceUpdateHandler(context);
        var command = new BulkPriceUpdateCommand
        {
            PercentageIncrease = 0,
            PriceType = PriceType.Retail,
            CurrencyId = 1
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("درصد افزایش باید بزرگتر از صفر باشد", result.MessageSingle);
    }
}
