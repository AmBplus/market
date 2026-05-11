using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Domain.Order;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Orders.Commands;

public class CreateOrderCommand
{
    public long UserId { get; set; }
    public long DeliveryMethodId { get; set; }
    public string ShippingAddressJson { get; set; } = string.Empty;
    public string? CustomerNote { get; set; }
}

public class CreateOrderHandler
{
    private readonly ECommerceDbContext _context;
    public CreateOrderHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<long>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation<long>.ToFailedResult("شناسه کاربر نامعتبر است");

        if (command.DeliveryMethodId <= 0)
            return ResultOperation<long>.ToFailedResult("شناسه روش ارسال نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.ShippingAddressJson))
            return ResultOperation<long>.ToFailedResult("آدرس ارسال الزامی است");

        // Get active cart
        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.ProductVariant)
            .FirstOrDefaultAsync(c => c.UserId == command.UserId && c.IsActive, ct);

        if (cart is null || !cart.Items.Any())
            return ResultOperation<long>.ToFailedResult("سبد خرید خالی است");

        // Get delivery method
        var deliveryMethod = await _context.DeliveryMethods
            .FirstOrDefaultAsync(dm => dm.Id == command.DeliveryMethodId && dm.IsActive, ct);

        if (deliveryMethod is null)
            return ResultOperation<long>.ToFailedResult("روش ارسال یافت نشد یا غیرفعال است");

        // Calculate totals
        var subTotal = cart.Items.Sum(ci => ci.UnitPrice * ci.Quantity);
        var shippingCost = deliveryMethod.Price;
        var discountAmount = 0m;
        var taxAmount = 0m;

        // Apply discount if exists
        if (!string.IsNullOrWhiteSpace(cart.DiscountCode))
        {
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.Code == cart.DiscountCode && d.IsActive, ct);

            if (discount is not null)
            {
                discountAmount = discount.DiscountType == DiscountType.Percentage
                    ? Math.Min(subTotal * discount.Value / 100, discount.MaxDiscountAmount ?? decimal.MaxValue)
                    : discount.DiscountType == DiscountType.FixedAmount
                        ? Math.Min(discount.Value, discount.MaxDiscountAmount ?? decimal.MaxValue)
                        : 0;

                if (discount.MinOrderAmount.HasValue && subTotal < discount.MinOrderAmount.Value)
                    discountAmount = 0;
            }
        }

        var totalAmount = subTotal - discountAmount + shippingCost + taxAmount;

        // Generate order number
        var orderCount = await _context.Orders.CountAsync(ct);
        var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{(orderCount + 1):D6}";

        // Get first warehouse for stock reservation
        var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.IsActive, ct);

        var order = new Domain.Order.Order
        {
            OrderNumber = orderNumber,
            UserId = command.UserId,
            Status = OrderStatus.Pending,
            SubTotal = subTotal,
            DiscountAmount = discountAmount,
            ShippingCost = shippingCost,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            CurrencyId = cart.Items.First().CurrencyId,
            ShippingAddressJson = command.ShippingAddressJson,
            CustomerNote = command.CustomerNote,
            Items = cart.Items.Select(ci => new OrderItem
            {
                ProductVariantId = ci.ProductVariantId,
                ProductName = ci.ProductVariant.Product.Name,
                VariantTitle = ci.ProductVariant.Title,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                DiscountAmount = 0,
                TotalPrice = ci.UnitPrice * ci.Quantity,
                VendorId = ci.ProductVariant.VendorId,
                VendorCommissionRate = ci.ProductVariant.VendorId.HasValue ? ci.ProductVariant.Vendor.CommissionRate : null
            }).ToList()
        };

        _context.Add(order);

        // Create stock reservations
        if (warehouse is not null)
        {
            foreach (var ci in cart.Items)
            {
                var reservation = new StockReservation
                {
                    ProductVariantId = ci.ProductVariantId,
                    WarehouseId = warehouse.Id,
                    OrderId = order.Id,
                    Quantity = ci.Quantity,
                    ExpiresAt = DateTime.UtcNow.AddDays(3),
                    IsReleased = false
                };
                _context.Add(reservation);

                var stockLedger = new StockLedger
                {
                    ProductVariantId = ci.ProductVariantId,
                    WarehouseId = warehouse.Id,
                    QuantityChange = -ci.Quantity,
                    Reason = StockChangeReason.Reserved,
                    ReferenceId = order.Id
                };
                _context.Add(stockLedger);
            }
        }

        // Add order status history
        var statusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            FromStatus = OrderStatus.Pending,
            ToStatus = OrderStatus.Pending,
            Note = "سفارش ایجاد شد"
        };
        _context.Add(statusHistory);

        // Clear cart
        _context.CartItems.RemoveRange(cart.Items);
        cart.IsActive = false;

        await _context.SaveChangesAsync(ct);

        return ResultOperation<long>.ToSuccessResult(order.Id, "سفارش با موفقیت ایجاد شد");
    }
}
