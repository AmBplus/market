using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Settings.EnumValues.Queries;

public class GetEnumValuesByTypeQuery
{
    public string EnumType { get; set; } = string.Empty;
}

public class EnumValueDto
{
    public long Id { get; set; }
    public string EnumType { get; set; } = string.Empty;
    public string EnumKey { get; set; } = string.Empty;
    public int EnumId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class GetEnumValuesByTypeHandler
{
    private readonly ECommerceDbContext _context;
    public GetEnumValuesByTypeHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<EnumValueDto>>> Handle(GetEnumValuesByTypeQuery query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.EnumType))
            return ResultOperation<List<EnumValueDto>>.ToFailedResult("نوع enum الزامی است");

        var result = await _context.EnumValues
            .AsNoTracking()
            .Where(e => e.EnumType == query.EnumType && e.IsActive)
            .OrderBy(e => e.DisplayOrder)
            .Select(e => new EnumValueDto
            {
                Id = e.Id,
                EnumType = e.EnumType,
                EnumKey = e.EnumKey,
                EnumId = e.EnumId,
                Name = e.Name,
                Description = e.Description,
                DisplayOrder = e.DisplayOrder,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);

        return ResultOperation<List<EnumValueDto>>.ToSuccessResult(result);
    }
}
