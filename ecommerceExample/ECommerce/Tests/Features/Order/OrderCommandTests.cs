using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Identity;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Domain.Order;
using ECommerce.AppCore.Domain.Pricing;
using ECommerce.AppCore.Domain.Product;
using ECommerce.AppCore.Enums;
using ECommerce.AppCore.Features.Order.Carts.Commands;
using ECommerce.AppCore.Features.Order.Orders.Commands;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace Tests.Features.Order;

public class OrderCommandTests
{
    private async Task<ECommerceDbContext> GetDbContextWithSeedAsync()
    {
        var context = TestHelper.GetInMemoryDbContext();

        // Seed user
        context.Users.Add(new User
        {
            UserName = "orderuser",
            Email = "order@example.com",
            FirstName = "کاربر",
            LastName = "سفارش",
            PasswordHash = "hash",
            PasswordSalt = "salt",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

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

        // Seed delivery method
        context.DeliveryMethods.Add(new Domain.Shipping.DeliveryMethod
        {
            Name = "ارسال پستی",
            DeliveryType = DeliveryType.Physical,
            Price = 50000m,
            CurrencyId = 1,
            EstimatedDays = 3,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        // Seed product + variant
        var product = new Domain.Product.Product
        {
            ProductCode = "ORD-PROD-001",
            Name = "گوشی موبایل",
            Slug = "mobile-phone",
            IsActive = true,
            IsFeatured = false,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var variant = new ProductVariant
        {
            ProductId = product.Id,
            VariantCode = "VAR-ORD-001",
            Sku = "SKU-ORD-001",
            Title = "مشکی - 128 گیگ",
            DefaultPrice = 15000000,
            CurrencyId = 1,
            IsActive = true,
            IsDeleted = false
        };
        context.ProductVariants.Add(variant);
        await context.SaveChangesAsync();

        // Add initial stock
        context.StockLedgers.Add(new StockLedger
        {
            ProductVariantId = variant.Id,
            WarehouseId = 1,
            QuantityChange = 100,
            Reason = StockChangeReason.Initial,
            Description = "موجودی اولیه"
        });
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task CreateOrder_WithCartItems_ReturnsSuccessResultWithOrderId()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        // Add product to cart first
        var addToCartHandler = new AddToCartHandler(context);
        await addToCartHandler.Handle(new AddToCartCommand
        {
            UserId = 1,
            ProductVariantId = 1,
            Quantity = 2
        }, CancellationToken.None);

        var createOrderHandler = new CreateOrderHandler(context);
        var command = new CreateOrderCommand
        {
            UserId = 1,
            DeliveryMethodId = 1,
            ShippingAddressJson = """{"city":"تهران","street":"ولیعصر","number":"123"}""",
            CustomerNote = "لطفاً بسته‌بندی هدیه باشد"
        };

        // Act
        var result = await createOrderHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data > 0);

        var order = await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == result.Data);
        Assert.NotNull(order);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(1, order.UserId);
        Assert.NotEmpty(order.OrderNumber);
        Assert.True(order.SubTotal > 0);
        Assert.True(order.ShippingCost == 50000m);
        Assert.Single(order.Items);

        // Cart should be deactivated
        var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == 1);
        Assert.NotNull(cart);
        Assert.False(cart.IsActive);

        // Stock reservation should exist
        var reservation = await context.StockReservations.FirstOrDefaultAsync();
        Assert.NotNull(reservation);
        Assert.Equal(2, reservation.Quantity);

        // Stock ledger for reservation should exist
        var ledger = await context.StockLedgers
            .FirstOrDefaultAsync(sl => sl.Reason == StockChangeReason.Reserved);
        Assert.NotNull(ledger);
        Assert.Equal(-2, ledger.QuantityChange);
    }

    [Fact]
    public async Task CreateOrder_EmptyCart_ReturnsFailedResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreateOrderHandler(context);
        var command = new CreateOrderCommand
        {
            UserId = 1,
            DeliveryMethodId = 1,
            ShippingAddressJson = """{"city":"تهران"}"""
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("سبد خرید خالی است", result.MessageSingle);
    }

    [Fact]
    public async Task CancelOrder_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        // Create order with cart items
        var addToCartHandler = new AddToCartHandler(context);
        await addToCartHandler.Handle(new AddToCartCommand
        {
            UserId = 1,
            ProductVariantId = 1,
            Quantity = 3
        }, CancellationToken.None);

        var createOrderHandler = new CreateOrderHandler(context);
        var orderResult = await createOrderHandler.Handle(new CreateOrderCommand
        {
            UserId = 1,
            DeliveryMethodId = 1,
            ShippingAddressJson = """{"city":"تهران"}"""
        }, CancellationToken.None);

        var cancelHandler = new CancelOrderHandler(context);
        var cancelCommand = new CancelOrderCommand
        {
            OrderId = orderResult.Data,
            Reason = "انصراف مشتری"
        };

        // Act
        var result = await cancelHandler.Handle(cancelCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("سفارش با موفقیت لغو شد", result.MessageSingle);

        var order = await context.Orders.FindAsync(orderResult.Data);
        Assert.NotNull(order);
        Assert.Equal(OrderStatus.Cancelled, order.Status);

        // Stock reservations should be released
        var reservation = await context.StockReservations.FirstOrDefaultAsync(r => r.OrderId == orderResult.Data);
        Assert.NotNull(reservation);
        Assert.True(reservation.IsReleased);
        Assert.NotNull(reservation.ReleasedAt);

        // Stock ledger for release should exist
        var releaseLedger = await context.StockLedgers
            .FirstOrDefaultAsync(sl => sl.Reason == StockChangeReason.ReservationReleased);
        Assert.NotNull(releaseLedger);
        Assert.Equal(3, releaseLedger.QuantityChange);

        // Status history should exist
        var statusHistory = await context.OrderStatusHistories
            .FirstOrDefaultAsync(sh => sh.OrderId == orderResult.Data && sh.ToStatus == OrderStatus.Cancelled);
        Assert.NotNull(statusHistory);
    }

    [Fact]
    public async Task CancelOrder_NonPendingOrder_ReturnsFailedResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();

        var addToCartHandler = new AddToCartHandler(context);
        await addToCartHandler.Handle(new AddToCartCommand
        {
            UserId = 1,
            ProductVariantId = 1,
            Quantity = 1
        }, CancellationToken.None);

        var createOrderHandler = new CreateOrderHandler(context);
        var orderResult = await createOrderHandler.Handle(new CreateOrderCommand
        {
            UserId = 1,
            DeliveryMethodId = 1,
            ShippingAddressJson = """{"city":"تهران"}"""
        }, CancellationToken.None);

        // First cancel
        var cancelHandler = new CancelOrderHandler(context);
        await cancelHandler.Handle(new CancelOrderCommand { OrderId = orderResult.Data }, CancellationToken.None);

        // Second cancel attempt
        var result = await cancelHandler.Handle(new CancelOrderCommand { OrderId = orderResult.Data }, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("فقط سفارشات در وضعیت انتظار قابل لغو هستند", result.MessageSingle);
    }
}
