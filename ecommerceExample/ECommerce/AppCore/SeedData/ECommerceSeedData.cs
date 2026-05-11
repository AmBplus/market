using System.Security.Cryptography;
using System.Text;
using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Accounting;
using ECommerce.AppCore.Domain.Customization;
using ECommerce.AppCore.Domain.Identity;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Domain.Payment;
using ECommerce.AppCore.Domain.Pricing;
using ECommerce.AppCore.Domain.Settings;
using ECommerce.AppCore.Domain.Shipping;
using ECommerce.AppCore.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.SeedData;

public static class ECommerceSeedData
{
    public static async Task SeedAsync(ECommerceDbContext context)
    {
        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Seed in dependency order
        await SeedCurrenciesAsync(context);
        await SeedSettingsAsync(context);
        await SeedEnumValuesAsync(context);
        await SeedPaymentGatewaysAsync(context);
        await SeedWarehousesAsync(context);
        await SeedAccountsAsync(context);
        await SeedDeliveryMethodsAsync(context);
        await SeedSiteThemeAsync(context);
        await SeedSlidersAsync(context);
        await SeedAdminUserAsync(context);
    }

    private static async Task SeedCurrenciesAsync(ECommerceDbContext context)
    {
        if (!await context.Currencies.AnyAsync())
        {
            context.Currencies.AddRange(
                new Currency
                {
                    Code = "IRR", Name = "تومان", Symbol = "ت",
                    ExchangeRate = 1.0m, IsBase = true, IsActive = true, CreatedAt = DateTime.UtcNow
                },
                new Currency
                {
                    Code = "USD", Name = "دلار آمریکا", Symbol = "$",
                    ExchangeRate = 50000m, IsBase = false, IsActive = true, CreatedAt = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSettingsAsync(ECommerceDbContext context)
    {
        if (!await context.Settings.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.Settings.AddRange(
                new Setting { Key = "DefaultCurrencyId", Value = "1", Module = "Pricing", Description = "شناسه ارز پیش‌فرض سیستم", ValueType = "Long", IsEditable = true, CreatedAt = now },
                new Setting { Key = "SiteName", Value = "فروشگاه آنلاین", Module = "General", Description = "نام فروشگاه", ValueType = "String", IsEditable = true, CreatedAt = now },
                new Setting { Key = "SiteDescription", Value = "فروشگاه آنلاین با بهترین محصولات و قیمت‌ها", Module = "General", Description = "توضیحات فروشگاه", ValueType = "String", IsEditable = true, CreatedAt = now },
                new Setting { Key = "OrderPrefix", Value = "ORD", Module = "Order", Description = "پیشوند شماره سفارش", ValueType = "String", IsEditable = true, CreatedAt = now },
                new Setting { Key = "TaxRate", Value = "0.09", Module = "Pricing", Description = "نرخ مالیات بر ارزش افزوده", ValueType = "Decimal", IsEditable = true, CreatedAt = now },
                new Setting { Key = "FreeShippingThreshold", Value = "500000", Module = "Shipping", Description = "حداقل مبلغ سفارش برای ارسال رایگان (تومان)", ValueType = "Decimal", IsEditable = true, CreatedAt = now },
                new Setting { Key = "DefaultWarehouseId", Value = "1", Module = "Inventory", Description = "شناسه انبار پیش‌فرض", ValueType = "Long", IsEditable = true, CreatedAt = now },
                new Setting { Key = "LowStockThreshold", Value = "5", Module = "Inventory", Description = "آستانه هشدار کمبود موجودی", ValueType = "Int", IsEditable = true, CreatedAt = now },
                new Setting { Key = "AdminEmail", Value = "admin@example.com", Module = "General", Description = "ایمیل مدیر سیستم", ValueType = "String", IsEditable = true, CreatedAt = now },
                new Setting { Key = "EnableRegistration", Value = "true", Module = "Identity", Description = "فعال‌سازی ثبت‌نام کاربران", ValueType = "Boolean", IsEditable = true, CreatedAt = now },
                new Setting { Key = "EnableGuestCheckout", Value = "true", Module = "Order", Description = "فعال‌سازی خرید بدون حساب کاربری", ValueType = "Boolean", IsEditable = true, CreatedAt = now },
                new Setting { Key = "VendorCommissionRate", Value = "0.10", Module = "Vendor", Description = "نرخ کمیسیون فروشنده", ValueType = "Decimal", IsEditable = true, CreatedAt = now },
                new Setting { Key = "MaxCartItems", Value = "50", Module = "Order", Description = "حداکثر آیتم‌های سبد خرید", ValueType = "Int", IsEditable = true, CreatedAt = now },
                new Setting { Key = "SessionTimeoutMinutes", Value = "30", Module = "General", Description = "مدت زمان نشست کاربر (دقیقه)", ValueType = "Int", IsEditable = true, CreatedAt = now },
                new Setting { Key = "PaymentTimeoutMinutes", Value = "15", Module = "Payment", Description = "مدت زمان اعتبار پرداخت (دقیقه)", ValueType = "Int", IsEditable = true, CreatedAt = now }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedEnumValuesAsync(ECommerceDbContext context)
    {
        if (!await context.EnumValues.AnyAsync())
        {
            var now = DateTime.UtcNow;
            var enumValues = new List<EnumValue>();

            // OrderStatus
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Pending", EnumId = (int)OrderStatus.Pending, Name = "در انتظار", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Paid", EnumId = (int)OrderStatus.Paid, Name = "پرداخت شده", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Processing", EnumId = (int)OrderStatus.Processing, Name = "در حال پردازش", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Shipped", EnumId = (int)OrderStatus.Shipped, Name = "ارسال شده", DisplayOrder = 4, IsActive = true },
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Delivered", EnumId = (int)OrderStatus.Delivered, Name = "تحویل داده شده", DisplayOrder = 5, IsActive = true },
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Cancelled", EnumId = (int)OrderStatus.Cancelled, Name = "لغو شده", DisplayOrder = 6, IsActive = true },
                new EnumValue { EnumType = nameof(OrderStatus), EnumKey = "Refunded", EnumId = (int)OrderStatus.Refunded, Name = "بازپرداخت شده", DisplayOrder = 7, IsActive = true },
            });

            // PaymentStatus
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(PaymentStatus), EnumKey = "Created", EnumId = (int)PaymentStatus.Created, Name = "ایجاد شده", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(PaymentStatus), EnumKey = "Pending", EnumId = (int)PaymentStatus.Pending, Name = "در انتظار پرداخت", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(PaymentStatus), EnumKey = "Succeeded", EnumId = (int)PaymentStatus.Succeeded, Name = "موفق", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(PaymentStatus), EnumKey = "Failed", EnumId = (int)PaymentStatus.Failed, Name = "ناموفق", DisplayOrder = 4, IsActive = true },
                new EnumValue { EnumType = nameof(PaymentStatus), EnumKey = "Refunded", EnumId = (int)PaymentStatus.Refunded, Name = "بازپرداخت شده", DisplayOrder = 5, IsActive = true },
            });

            // DeliveryStatus
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(DeliveryStatus), EnumKey = "Pending", EnumId = (int)DeliveryStatus.Pending, Name = "در انتظار ارسال", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(DeliveryStatus), EnumKey = "InTransit", EnumId = (int)DeliveryStatus.InTransit, Name = "در مسیر", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(DeliveryStatus), EnumKey = "Delivered", EnumId = (int)DeliveryStatus.Delivered, Name = "تحویل داده شده", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(DeliveryStatus), EnumKey = "Failed", EnumId = (int)DeliveryStatus.Failed, Name = "ناموفق", DisplayOrder = 4, IsActive = true },
            });

            // DeliveryType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(DeliveryType), EnumKey = "Physical", EnumId = (int)DeliveryType.Physical, Name = "ارسال پستی", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(DeliveryType), EnumKey = "InPerson", EnumId = (int)DeliveryType.InPerson, Name = "تحویل حضوری", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(DeliveryType), EnumKey = "Digital", EnumId = (int)DeliveryType.Digital, Name = "محتوای دیجیتال", DisplayOrder = 3, IsActive = true },
            });

            // DigitalContentType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(DigitalContentType), EnumKey = "Link", EnumId = (int)DigitalContentType.Link, Name = "لینک دانلود", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(DigitalContentType), EnumKey = "File", EnumId = (int)DigitalContentType.File, Name = "فایل دانلودی", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(DigitalContentType), EnumKey = "Content", EnumId = (int)DigitalContentType.Content, Name = "محتوای متنی", DisplayOrder = 3, IsActive = true },
            });

            // PriceType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(PriceType), EnumKey = "Retail", EnumId = (int)PriceType.Retail, Name = "خرده‌فروشی", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(PriceType), EnumKey = "Wholesale", EnumId = (int)PriceType.Wholesale, Name = "عمده‌فروشی", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(PriceType), EnumKey = "Special", EnumId = (int)PriceType.Special, Name = "ویژه", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(PriceType), EnumKey = "Cost", EnumId = (int)PriceType.Cost, Name = "قیمت تمام شده", DisplayOrder = 4, IsActive = true },
            });

            // StockChangeReason
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "StockIn", EnumId = (int)StockChangeReason.StockIn, Name = "ورود کالا", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "StockOut", EnumId = (int)StockChangeReason.StockOut, Name = "خروج کالا", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "Return", EnumId = (int)StockChangeReason.Return, Name = "مرجوعی", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "Damaged", EnumId = (int)StockChangeReason.Damaged, Name = "کالای آسیب‌دیده", DisplayOrder = 4, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "Initial", EnumId = (int)StockChangeReason.Initial, Name = "موجودی اولیه", DisplayOrder = 5, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "ManualAdjustment", EnumId = (int)StockChangeReason.ManualAdjustment, Name = "تعدیل دستی", DisplayOrder = 6, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "Reserved", EnumId = (int)StockChangeReason.Reserved, Name = "رزرو شده", DisplayOrder = 7, IsActive = true },
                new EnumValue { EnumType = nameof(StockChangeReason), EnumKey = "ReservationReleased", EnumId = (int)StockChangeReason.ReservationReleased, Name = "آزادسازی رزرو", DisplayOrder = 8, IsActive = true },
            });

            // DiscountType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(DiscountType), EnumKey = "Percentage", EnumId = (int)DiscountType.Percentage, Name = "درصدی", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(DiscountType), EnumKey = "FixedAmount", EnumId = (int)DiscountType.FixedAmount, Name = "مبلغ ثابت", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(DiscountType), EnumKey = "FreeShipping", EnumId = (int)DiscountType.FreeShipping, Name = "ارسال رایگان", DisplayOrder = 3, IsActive = true },
            });

            // DiscountScope
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(DiscountScope), EnumKey = "Order", EnumId = (int)DiscountScope.Order, Name = "سفارش", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(DiscountScope), EnumKey = "Product", EnumId = (int)DiscountScope.Product, Name = "محصول", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(DiscountScope), EnumKey = "Category", EnumId = (int)DiscountScope.Category, Name = "دسته‌بندی", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(DiscountScope), EnumKey = "Shipping", EnumId = (int)DiscountScope.Shipping, Name = "هزینه ارسال", DisplayOrder = 4, IsActive = true },
            });

            // ShippingConstraintType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(ShippingConstraintType), EnumKey = "Fragile", EnumId = (int)ShippingConstraintType.Fragile, Name = "شکننده", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(ShippingConstraintType), EnumKey = "Heavy", EnumId = (int)ShippingConstraintType.Heavy, Name = "سنگین", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(ShippingConstraintType), EnumKey = "Oversized", EnumId = (int)ShippingConstraintType.Oversized, Name = "بزرگ‌تر از حد معمول", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(ShippingConstraintType), EnumKey = "Hazardous", EnumId = (int)ShippingConstraintType.Hazardous, Name = "خطرناک", DisplayOrder = 4, IsActive = true },
                new EnumValue { EnumType = nameof(ShippingConstraintType), EnumKey = "Perishable", EnumId = (int)ShippingConstraintType.Perishable, Name = "فاسدشدنی", DisplayOrder = 5, IsActive = true },
            });

            // AccountType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(AccountType), EnumKey = "Asset", EnumId = (int)AccountType.Asset, Name = "دارایی", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(AccountType), EnumKey = "Liability", EnumId = (int)AccountType.Liability, Name = "بدهی", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(AccountType), EnumKey = "Revenue", EnumId = (int)AccountType.Revenue, Name = "درآمد", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(AccountType), EnumKey = "Expense", EnumId = (int)AccountType.Expense, Name = "هزینه", DisplayOrder = 4, IsActive = true },
                new EnumValue { EnumType = nameof(AccountType), EnumKey = "Equity", EnumId = (int)AccountType.Equity, Name = "حقوق صاحبان سهام", DisplayOrder = 5, IsActive = true },
            });

            // TransactionType
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(TransactionType), EnumKey = "Debit", EnumId = (int)TransactionType.Debit, Name = "بدهکار", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(TransactionType), EnumKey = "Credit", EnumId = (int)TransactionType.Credit, Name = "بستانکار", DisplayOrder = 2, IsActive = true },
            });

            // TransactionSource
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(TransactionSource), EnumKey = "Order", EnumId = (int)TransactionSource.Order, Name = "سفارش", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(TransactionSource), EnumKey = "Refund", EnumId = (int)TransactionSource.Refund, Name = "بازپرداخت", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(TransactionSource), EnumKey = "Shipping", EnumId = (int)TransactionSource.Shipping, Name = "ارسال", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(TransactionSource), EnumKey = "VendorCommission", EnumId = (int)TransactionSource.VendorCommission, Name = "کمیسیون فروشنده", DisplayOrder = 4, IsActive = true },
                new EnumValue { EnumType = nameof(TransactionSource), EnumKey = "Operational", EnumId = (int)TransactionSource.Operational, Name = "عملیاتی", DisplayOrder = 5, IsActive = true },
                new EnumValue { EnumType = nameof(TransactionSource), EnumKey = "Manual", EnumId = (int)TransactionSource.Manual, Name = "دستی", DisplayOrder = 6, IsActive = true },
            });

            // VendorStatus
            enumValues.AddRange(new[]
            {
                new EnumValue { EnumType = nameof(VendorStatus), EnumKey = "Pending", EnumId = (int)VendorStatus.Pending, Name = "در انتظار تأیید", DisplayOrder = 1, IsActive = true },
                new EnumValue { EnumType = nameof(VendorStatus), EnumKey = "Active", EnumId = (int)VendorStatus.Active, Name = "فعال", DisplayOrder = 2, IsActive = true },
                new EnumValue { EnumType = nameof(VendorStatus), EnumKey = "Suspended", EnumId = (int)VendorStatus.Suspended, Name = "معلق", DisplayOrder = 3, IsActive = true },
                new EnumValue { EnumType = nameof(VendorStatus), EnumKey = "Rejected", EnumId = (int)VendorStatus.Rejected, Name = "رد شده", DisplayOrder = 4, IsActive = true },
            });

            context.EnumValues.AddRange(enumValues);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedPaymentGatewaysAsync(ECommerceDbContext context)
    {
        if (!await context.PaymentGateways.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.PaymentGateways.AddRange(
                new PaymentGateway
                {
                    Name = "زرین‌پال",
                    Code = "Zarinpal",
                    ConfigJson = """{"MerchantId":"","Sandbox":false,"CallbackUrl":"/api/payment/callback/zarinpal"}""",
                    Priority = 1,
                    IsActive = true,
                    CreatedAt = now
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedWarehousesAsync(ECommerceDbContext context)
    {
        if (!await context.Warehouses.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.Warehouses.AddRange(
                new Warehouse
                {
                    Name = "انبار مرکزی",
                    Code = "WH-MAIN",
                    Address = "تهران، خیابان ولیعصر، پلاک ۱۲۳",
                    City = "تهران",
                    Province = "تهران",
                    IsActive = true,
                    CreatedAt = now
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedAccountsAsync(ECommerceDbContext context)
    {
        if (!await context.Accounts.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.Accounts.AddRange(
                new Account { Code = "1001", Name = "صندوق نقدی", AccountType = AccountType.Asset, Description = "حساب صندوق نقدی فروشگاه", IsActive = true, CreatedAt = now },
                new Account { Code = "1002", Name = "حساب بانکی", AccountType = AccountType.Asset, Description = "حساب بانکی فروشگاه (ملت)", IsActive = true, CreatedAt = now },
                new Account { Code = "4001", Name = "درآمد فروش", AccountType = AccountType.Revenue, Description = "درآمد حاصل از فروش محصولات", IsActive = true, CreatedAt = now },
                new Account { Code = "5001", Name = "بهای تمام شده کالای فروش رفته", AccountType = AccountType.Expense, Description = "هزینه خرید کالاهای فروخته شده", IsActive = true, CreatedAt = now },
                new Account { Code = "4002", Name = "درآمد ارسال", AccountType = AccountType.Revenue, Description = "درآمد حاصل از هزینه ارسال", IsActive = true, CreatedAt = now },
                new Account { Code = "2001", Name = "پرداختی فروشندگان", AccountType = AccountType.Liability, Description = "بدهی به فروشندگان برای کمیسیون و فروش محصولات", IsActive = true, CreatedAt = now }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedDeliveryMethodsAsync(ECommerceDbContext context)
    {
        if (!await context.DeliveryMethods.AnyAsync())
        {
            var now = DateTime.UtcNow;
            var baseCurrency = await context.Currencies.FirstOrDefaultAsync(c => c.IsBase);

            context.DeliveryMethods.AddRange(
                new DeliveryMethod
                {
                    Name = "ارسال پستی",
                    DeliveryType = DeliveryType.Physical,
                    Price = 50000m,
                    CurrencyId = baseCurrency?.Id ?? 1,
                    EstimatedDays = 3,
                    Description = "ارسال از طریق پست پیشتاز به سراسر کشور",
                    IsActive = true,
                    CreatedAt = now
                },
                new DeliveryMethod
                {
                    Name = "تحویل حضوری",
                    DeliveryType = DeliveryType.InPerson,
                    Price = 0m,
                    CurrencyId = baseCurrency?.Id ?? 1,
                    EstimatedDays = 0,
                    Description = "تحویل حضوری از فروشگاه",
                    IsActive = true,
                    CreatedAt = now
                },
                new DeliveryMethod
                {
                    Name = "لینک دانلود",
                    DeliveryType = DeliveryType.Digital,
                    Price = 0m,
                    CurrencyId = baseCurrency?.Id ?? 1,
                    EstimatedDays = 0,
                    Description = "ارسال لینک دانلود محصول دیجیتال",
                    DigitalContentType = DigitalContentType.Link,
                    IsActive = true,
                    CreatedAt = now
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSiteThemeAsync(ECommerceDbContext context)
    {
        if (!await context.SiteThemes.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.SiteThemes.AddRange(
                new SiteTheme
                {
                    Name = "پوسته پیش‌فرض",
                    PrimaryColor = "#3B82F6",
                    SecondaryColor = "#10B981",
                    BackgroundColor = "#FFFFFF",
                    TextColor = "#1F2937",
                    AccentColor = "#F59E0B",
                    FontFamily = "Vazirmatn, sans-serif",
                    ExtendedSettingsJson = """{"BorderRadius":"8px","DarkMode":false,"SidebarCollapsed":false}""",
                    IsActive = true,
                    CreatedAt = now
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSlidersAsync(ECommerceDbContext context)
    {
        if (!await context.Sliders.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.Sliders.AddRange(
                new Slider
                {
                    Title = "جشنواره فروش ویژه",
                    SubTitle = "تا ۵۰٪ تخفیف روی تمامی محصولات",
                    ImageUrl = "/images/sliders/sale-banner.jpg",
                    MobileImageUrl = "/images/sliders/sale-banner-mobile.jpg",
                    LinkUrl = "/products?discount=true",
                    LinkText = "مشاهده محصولات",
                    DisplayOrder = 1,
                    IsActive = true,
                    StartedAt = now,
                    EndedAt = now.AddDays(30),
                    CreatedAt = now
                },
                new Slider
                {
                    Title = "محصولات جدید فصل",
                    SubTitle = "جدیدترین محصولات با بهترین کیفیت",
                    ImageUrl = "/images/sliders/new-arrivals.jpg",
                    MobileImageUrl = "/images/sliders/new-arrivals-mobile.jpg",
                    LinkUrl = "/products?new=true",
                    LinkText = "کشف کنید",
                    DisplayOrder = 2,
                    IsActive = true,
                    StartedAt = now,
                    EndedAt = now.AddDays(60),
                    CreatedAt = now
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedAdminUserAsync(ECommerceDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            var now = DateTime.UtcNow;

            // Ensure Admin role exists
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole is null)
            {
                adminRole = new Role
                {
                    Name = "Admin",
                    DisplayName = "مدیر سیستم",
                    Description = "دسترسی کامل به تمام بخش‌های سیستم",
                    IsActive = true,
                    CreatedAt = now
                };
                context.Roles.Add(adminRole);
                await context.SaveChangesAsync();
            }

            var (hash, salt) = HashPassword("admin123");

            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@example.com",
                PhoneNumber = "09123456789",
                FirstName = "مدیر",
                LastName = "سیستم",
                PasswordHash = hash,
                PasswordSalt = salt,
                IsActive = true,
                CreatedAt = now
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            context.UserRoles.Add(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                AssignedAt = now
            });
            await context.SaveChangesAsync();
        }
    }

    private static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        var hash = Convert.ToBase64String(hashBytes);

        return (hash, salt);
    }
}
