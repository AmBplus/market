using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.Queries
{
    public class ColorDataTableQuery : DatatableFullRequest
    {
        public string? SearchTitle { get; set; }
        public string? SearchColorCode { get; set; }
    }

    public class ColorListDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class ColorDataTableHandler
    {
        private readonly AppDbContext _context;

        public ColorDataTableHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<DataTableResponse<ColorListDto>>> Handle(ColorDataTableQuery query)
        {
            // 1. بیزاس کوئری با فیلتر حذف‌شده‌ها
            var baseQuery = _context.Colors
                .AsNoTracking()
                .Where(c => !c.IsDelete)
                .AsQueryable();

            // 2. فیلترهای جستجو
            if (!string.IsNullOrWhiteSpace(query.SearchTitle))
                baseQuery = baseQuery.Where(c => c.Title.Contains(query.SearchTitle));

            if (!string.IsNullOrWhiteSpace(query.SearchColorCode))
                baseQuery = baseQuery.Where(c => c.ColorCode.Contains(query.SearchColorCode));

            // 3. شمارش کل رکوردها
            var filteredCount = await baseQuery.CountAsync();

            // 4. انتخاب به DTO
            var dtoQuery = baseQuery.Select(c => new ColorListDto
            {
                Id = c.Id,
                Title = c.Title,
                ColorCode = c.ColorCode,
                CreateAt = c.CreateAt,
                IsActive = c.IsActive
            });

            // 5. ترتیب‌سازی داینامیک
            var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

            // 6. صفحه‌بندی
            var colors = await orderedQuery
                .Skip(query.start)
                .Take(query.length)
                .ToListAsync();

            // 7. پاسخ
            var response = new DataTableResponse<ColorListDto>
            {
                Draw = query.draw,
                RecordsTotal = filteredCount,
                RecordsFiltered = filteredCount,
                Data = colors
            };

            return ResultOperation<DataTableResponse<ColorListDto>>.ToSuccessResult(response);
        }
    }
}
