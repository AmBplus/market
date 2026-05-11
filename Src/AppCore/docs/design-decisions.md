# تصمیمات طراحی سیستم فروشگاه ECommerce

## چرا Wolverine به جای MediatR

| معیار | MediatR | Wolverine |
|-------|---------|-----------|
| رزولوشن هندلر | Reflection-based | Source-generated |
| عملکرد | متوسط | بالا (compile-time) |
| پردازش ناهمزمان | نیاز به پکیج اضافی | داخلی |
| Middleware | Pipeline Behaviors | Message Handlers |
| پشتیبانی از Event Sourcing | ندارد | دارد |
| آینده .NET | توسعه محدود | فعال |

**دلایل انتخاب Wolverine:**
1. **عملکرد بالاتر**: Wolverine از Source Generation استفاده می‌کند و از reflection اجتناب می‌کند
2. **آمادگی برای مقیاس‌پذیری**: با تغییر مینیمال می‌توان پردازش را به صف پیام منتقل کرد
3. **سادگی تست**: هندلرها کلاس‌های ساده با سازنده هستند و به‌راحتی تست می‌شوند
4. **پشتیبانی داخلی از EF Core**: پکیج `Wolverine.EntityFrameworkCore` یکپارچگی بهتری ارائه می‌دهد

---

## چرا Append-Only برای موجودی و قیمت

### مشکل رویکرد Update-In-Place
```
❌ رویکرد سنتی:
UPDATE Inventory SET Quantity = 95 WHERE ProductVariantId = 1
-- تاریخچه از بین می‌رود!
-- در صورت همزمانی، Race Condition رخ می‌دهد
```

### راه‌حل Append-Only
```
✅ رویکرد Append-Only:
INSERT INTO StockLedgers (ProductVariantId, WarehouseId, QuantityChange, Reason)
VALUES (1, 1, -5, 'StockOut')

-- تاریخچه کامل حفظ می‌شود
-- محاسبه موجودی: SUM(QuantityChange) = 95
-- بدون Race Condition در INSERT
```

**مزایا:**
1. **تاریخچه کامل**: هر تغییری ثبت و قابل ردیابی است
2. **حسابرسی**: می‌توان هر لحظه موجودی را بازسازی کرد
3. **ضد Race Condition**: INSERT همزمان به‌جای UPDATE همزمان
4. **تحلیل آماری**: امکان محاسبه میانگین مصرف، روند خروج و...
5. **بازگشت‌پذیری**: با ثبت رکورد معکوس، عملیات قابل بازگشت است

### مشابه برای قیمت
قیمت‌ها نیز Append-Only هستند. قیمت قبلی بسته می‌شود (EndedAt) و قیمت جدید باز می‌شود. این روش:
- تاریخچه قیمت را حفظ می‌کند
- امکان نمایش «کاهش/افزایش قیمت» را فراهم می‌کند
- قیمت در زمان سفارش دقیقاً قابل بازیابی است

---

## چرا Fluent API به جای Data Annotations

### Data Annotations
```csharp
❌ [Required, MaxLength(100)]
   public string UserName { get; set; }
```

### Fluent API
```csharp
✅ builder.Property(u => u.UserName)
       .IsRequired()
       .HasMaxLength(100);
```

**دلایل انتخاب Fluent API:**
1. **تفکیک مسئولیت**: موجودیت دامنه نباید وابستگی به EF Core داشته باشد
2. **تنظیمات پیشرفته**: فیلتر ایندکس، اسکیما، نام ایندکس و... فقط با Fluent API قابل تنظیم‌اند
3. **نام‌گذاری ایندکس**: `HasDatabaseName("IX_Users_UserName")` برای کنترل دقیق
4. **فیلتر شده ایندکس**: `HasFilter("[Email] IS NOT NULL")` فقط با Fluent API
5. **تنظیمات ارتباطات**: `OnDelete(DeleteBehavior.Restrict)` و `SetNull`
6. **Query Filters**: `HasQueryFilter(p => !p.IsDeleted)` برای Soft Delete
7. **یکپارچگی**: تمام تنظیمات در یک فایل Configuration جمع شده‌اند

---

## چرا Soft Delete

```csharp
public abstract class SoftDeletableEntity : AuditableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
}
```

**دلایل:**
1. **حفظ یکپارچگی ارجاعی**: اگر محصول حذف شود، آیتم‌های سفارش قبلی همچنان به محصول اشاره معتبر دارند
2. **قابلیت بازگشت**: مدیر می‌تواند محصول حذف‌شده را بازیابی کند
3. **حسابرسی**: می‌توان فهمید چه چیزی، کِی و توسط چه کسی حذف شده
4. **تنظیمات EF Core**: با `HasQueryFilter` خودکار فیلتر می‌شود و نیاز به شرط دستی نیست

**جداول با Soft Delete:**
- Products
- ProductVariants

**توجه:** Soft Delete فقط برای جداولی استفاده شده که ممکن است در گذشته به آن‌ها ارجاع شده باشد (سفارشات، تراکنش‌ها و...). جداول کمکی مثل UserRole با Hard Delete حذف می‌شوند.

---

## چرا اسکیما تفکیکی

### بدون اسکیما (dbo پیش‌فرض)
```
❌ dbo.Users, dbo.Products, dbo.Prices, dbo.Orders, ...
-- 27 جدول در یک اسکیما → شلوغ و نامنظم
```

### با اسکیما تفکیکی
```
✅ Identity.Users, Product.Products, Pricing.Prices, Order.Orders, ...
-- هر ماژول اسکیما مستقل دارد
```

**دلایل:**
1. **سازمان‌دهی**: 27 جدول در 12 اسکیما مدیریت آسان‌تر از 27 جدول در یک اسکیما
2. **امنیت**: می‌توان دسترسی به اسکیمای Accounting را محدود کرد
3. **ماژولار بودن**: هر اسکیما نمایانگر یک bounded context است
4. **خوانایی**: `Pricing.Prices` بلافاصله مشخص می‌کند قیمت مربوط به ماژول قیمت‌گذاری است
5. **مستندسازی خودکار**: ساختار فیزیکی دیتابیس منطق کسب‌وکار را منعکس می‌کند

---

## چرا JSON برای آدرس‌ها

### رویکرد جداول تفکیکی
```
❌ Addresses, UserAddresses, OrderShippingAddresses
-- 3 جدول اضافی + JOIN‌های پیچیده
-- ساختار آدرس در ایران متغیر است
```

### رویکرد JSON
```csharp
✅ public string? DefaultAddressesJson { get; set; }     // User
✅ public string ShippingAddressJson { get; set; }        // Order
✅ public string? DeliveryAddressJson { get; set; }       // OrderDelivery
```

**دلایل:**
1. **انعطاف‌پذیری**: آدرس در ایران ساختار ثابتی ندارد (پلاک، واحد، کوچه،...)
2. **سادگی**: بدون نیاز به JOIN و موجودیت‌های اضافی
3. **ذخیره تاریخچه**: آدرس در زمان سفارش ثبت می‌شود و تغییر آدرس کاربر تأثیری بر سفارش قبلی ندارد
4. **SQL Server 2016+**: پشتیبانی بومی از JSON با `OPENJSON` و `JSON_VALUE`
5. **کاهش پیچیدگی**: 3 فیلد JSON به‌جای 3 جدول اضافی

---

## تفکیک مسائل اپلیکیشن و دیتابیس

### اپلیکیشن (C# Code)
- **اعتبارسنجی ورودی**: بررسی فیلدهای الزامی، فرمت و منطق کسب‌وکار
- **منطق کسب‌وکار**: محاسبه تخفیف، بررسی موجودی، تولید شماره سفارش
- **پیام‌های خطا**: پیام‌های فارسی کاربرپسند
- **ترتیب عملیات**: ترتیب ثبت رکوردها و وابستگی‌ها

### دیتابیس (SQL Server)
- **یکپارچگی ارجاعی**: Foreign Keys
- **یکتایی**: Unique Indexes
- **کارایی**: Composite Indexes برای کوئری‌های رایج
- **پیش‌فرض‌ها**: Default values برای ستون‌ها
- **فیلتر داده**: Filtered indexes برای مقادیر NULL

**مثال:**
```csharp
// اپلیکیشن: بررسی منطقی
if (currentStock < command.Quantity)
    return ResultOperation.ToFailedResult("موجودی کافی نیست");

// دیتابیس: تضمین یکتایی
builder.HasIndex(u => u.UserName).IsUnique();
```

---

## رویکرد ضد Race Condition

### مشکل Race Condition
```
Thread A: SELECT Quantity FROM Inventory WHERE Id=1  → 10
Thread B: SELECT Quantity FROM Inventory WHERE Id=1  → 10
Thread A: UPDATE Inventory SET Quantity = 5 WHERE Id=1  → OK
Thread B: UPDATE Inventory SET Quantity = 8 WHERE Id=1  → ❌ خطا!
```

### راه‌حل Append-Only
```
Thread A: INSERT INTO StockLedgers (QuantityChange) VALUES (-5)  → OK
Thread B: INSERT INTO StockLedgers (QuantityChange) VALUES (-2)  → OK
محاسبه: SUM(QuantityChange) = 3  → همیشه صحیح
```

**استراتژی‌های ضد Race Condition در سیستم:**

1. **Append-Only**: برای موجودی و قیمت، INSERT به‌جای UPDATE
2. **Stock Reservation**: رزرو موجودی هنگام سفارش با تاریخ انقضا
3. **بررسی موجودی قبل از خروج**: `StockOutCommand` موجودی فعلی را محاسبه و بررسی می‌کند
4. **ایندکس‌های فیلتر شده**: `IX_Prices_ProductVariantId_PriceType_EndedAt_Active` با فیلتر `[EndedAt] IS NULL` برای یافتن قیمت فعال بدون تداخل
5. **شماره سفارش یونیک**: تولید شماره سفارش بر اساس COUNT + تاریخ
6. **Soft Delete به‌جای Hard Delete**: حذف فیزیکی باعث تداخل در ارجاعات نمی‌شود
