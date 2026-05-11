using AppCore.Data;
using AppCore.Enums;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Order.Orders.Queries;

public class GetOrdersDataTableQuery : DatatableFullRequest
{
    public string? SearchOrderNumber { get; set; }
    public OrderStatus? SearchStatus { get; set; }
    public long? SearchUserId { get; set; }
    public DateTime? SearchFromDate { get; set; }
    public DateTime? SearchToDate { get; set; }
}

public class OrderListDto
{
    public long Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetOrdersDataTableHandler
{
    private readonly AppDbContext _context;
    public GetOrdersDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<OrderListDto>>> Handle(GetOrdersDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Orders.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchOrderNumber))
            baseQuery = baseQuery.Where(o => o.OrderNumber.Contains(query.SearchOrderNumber));

        if (query.SearchStatus.HasValue)
            baseQuery = baseQuery.Where(o => o.Status == query.SearchStatus.Value);

        if (query.SearchUserId.HasValue)
            baseQuery = baseQuery.Where(o => o.UserId == query.SearchUserId.Value);

        if (query.SearchFromDate.HasValue)
            baseQuery = baseQuery.Where(o => o.CreatedAt >= query.SearchFromDate.Value);

        if (query.SearchToDate.HasValue)
            baseQuery = baseQuery.Where(o => o.CreatedAt <= query.SearchToDate.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(o => new OrderListDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                UserName = o.User.UserName ?? "",
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<OrderListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<OrderListDto>>.ToSuccessResult(response);
    }
}
