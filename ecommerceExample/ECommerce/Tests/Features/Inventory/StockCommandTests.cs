using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Domain.Pricing;
using ECommerce.AppCore.Domain.Product;
using ECommerce.AppCore.Enums;
using ECommerce.AppCore.Features.Inventory.StockLedgers.Commands;
using ECommerce.AppCore.Features.Inventory.StockLedgers.Queries;
using Framework.ResultHelper;

namespace Tests.Features.Inventory;

public class StockCommandTests
{
    private async Task<ECommerceDbContext> GetDbContextWithSeedAsync()
    {
        var context = TestHelper.GetInMemoryDbContext();

        // Seed currency
        context.Currencies.Add(new Currency
        {
            Code = "IRR",
            Name = "تومان",
            Symbol = "ت",
            ExchangeRate = 1.0m,
            IsBase = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        // Seed warehouse
        context.Warehouses.Add(new Warehouse
        {
            Name = "انبار مرکزی",
            Code = "WH-MAIN",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        // Seed product + variant
        var product = new Domain.Product.Product
        {
            ProductCode = "STK-PROD-001",
            Name = "محصول تستی",
            Slug = "test-product",
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        context.ProductVariants.Add(new ProductVariant
        {
            ProductId = product.Id,
            VariantCode = "STK-VAR-001",
            Sku = "STK-SKU-001",
            Title = "تنوع تستی",
            DefaultPrice = 500000,
            CurrencyId = 1,
            IsActive = true,
            IsDeleted = false
        });
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task StockIn_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new StockInHandler(context);
        var command = new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 50,
            Description = "ورودی کالا از تأمین‌کننده"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("ورودی موجودی با موفقیت ثبت شد", result.MessageSingle);

        var ledger = context.StockLedgers.Last();
        Assert.Equal(50, ledger.QuantityChange);
        Assert.Equal(StockChangeReason.StockIn, ledger.Reason);
        Assert.Equal("ورودی کالا از تأمین‌کننده", ledger.Description);
    }

    [Fact]
    public async Task StockOut_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        // First add stock
        var stockInHandler = new StockInHandler(context);
        await stockInHandler.Handle(new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 100
        }, CancellationToken.None);

        var stockOutHandler = new StockOutHandler(context);
        var command = new StockOutCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 30,
            Description = "فروش آنلاین"
        };

        // Act
        var result = await stockOutHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("خروجی موجودی با موفقیت ثبت شد", result.MessageSingle);

        var lastLedger = context.StockLedgers.Last();
        Assert.Equal(-30, lastLedger.QuantityChange);
        Assert.Equal(StockChangeReason.StockOut, lastLedger.Reason);
    }

    [Fact]
    public async Task StockOut_InsufficientStock_ReturnsFailedResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        // Add small stock
        var stockInHandler = new StockInHandler(context);
        await stockInHandler.Handle(new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 10
        }, CancellationToken.None);

        var stockOutHandler = new StockOutHandler(context);
        var command = new StockOutCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 50 // More than available
        };

        // Act
        var result = await stockOutHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("موجودی کافی نیست", result.MessageSingle);
    }

    [Fact]
    public async Task AdjustStock_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        // Add initial stock
        var stockInHandler = new StockInHandler(context);
        await stockInHandler.Handle(new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 100
        }, CancellationToken.None);

        var adjustHandler = new AdjustStockHandler(context);
        var command = new AdjustStockCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            NewQuantity = 80, // Reduce by 20
            Description = "تعدیل پس از شمارش فیزیکی"
        };

        // Act
        var result = await adjustHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("تعدیل موجودی با موفقیت ثبت شد", result.MessageSingle);

        var lastLedger = context.StockLedgers.Last();
        Assert.Equal(-20, lastLedger.QuantityChange);
        Assert.Equal(StockChangeReason.ManualAdjustment, lastLedger.Reason);
    }

    [Fact]
    public async Task AdjustStock_NoChange_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        var stockInHandler = new StockInHandler(context);
        await stockInHandler.Handle(new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 100
        }, CancellationToken.None);

        var adjustHandler = new AdjustStockHandler(context);
        var command = new AdjustStockCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            NewQuantity = 100 // Same as current
        };

        // Act
        var result = await adjustHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("موجودی تغییر نکرد", result.MessageSingle);

        // No new ledger should be created
        Assert.Equal(2, context.StockLedgers.Count()); // Only the stock-in ledger + initial
    }

    [Fact]
    public async Task GetCurrentStock_ReturnsCorrectQuantity()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        var stockInHandler = new StockInHandler(context);
        await stockInHandler.Handle(new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 100
        }, CancellationToken.None);

        var stockOutHandler = new StockOutHandler(context);
        await stockOutHandler.Handle(new StockOutCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 30
        }, CancellationToken.None);

        var queryHandler = new GetCurrentStockHandler(context);
        var query = new GetCurrentStockQuery
        {
            ProductVariantId = 1,
            WarehouseId = 1
        };

        // Act
        var result = await queryHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(70, result.Data.Quantity); // 100 - 30 = 70
        Assert.Equal(1, result.Data.ProductVariantId);
        Assert.Equal(1, result.Data.WarehouseId);
    }

    [Fact]
    public async Task GetCurrentStock_WithoutWarehouseId_ReturnsTotalQuantity()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        // Add stock to the only warehouse
        var stockInHandler = new StockInHandler(context);
        await stockInHandler.Handle(new StockInCommand
        {
            ProductVariantId = 1,
            WarehouseId = 1,
            Quantity = 50
        }, CancellationToken.None);

        var queryHandler = new GetCurrentStockHandler(context);
        var query = new GetCurrentStockQuery
        {
            ProductVariantId = 1
            // No WarehouseId - should sum across all warehouses
        };

        // Act
        var result = await queryHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(50, result.Data.Quantity);
    }
}
