using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Orders.Queries;

public class GetOrderByIdQuery
{
    public long Id { get; set; }
}

public class OrderItemDto
{
    public long ProductVariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string VariantTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
}

public class OrderDto
{
    public long Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public string? CustomerNote { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetOrderByIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetOrderByIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<OrderDto>> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<OrderDto>.ToFailedResult("شناسه سفارش نامعتبر است");

        var order = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == query.Id)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Status = o.Status,
                SubTotal = o.SubTotal,
                DiscountAmount = o.DiscountAmount,
                ShippingCost = o.ShippingCost,
                TaxAmount = o.TaxAmount,
                TotalAmount = o.TotalAmount,
                CurrencyCode = o.Currency.Code,
                Items = o.Items.Select(oi => new OrderItemDto
                {
                    ProductVariantId = oi.ProductVariantId,
                    ProductName = oi.ProductName,
                    VariantTitle = oi.VariantTitle,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    DiscountAmount = oi.DiscountAmount,
                    TotalPrice = oi.TotalPrice
                }).ToList(),
                CustomerNote = o.CustomerNote,
                CreatedAt = o.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (order is null)
            return ResultOperation<OrderDto>.ToFailedResult("سفارش یافت نشد");

        return ResultOperation<OrderDto>.ToSuccessResult(order);
    }
}
