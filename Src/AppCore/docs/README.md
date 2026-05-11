# فروشگاه آنلاین ECommerce

سیستم فروشگاه آنلاین چندمنظوره با معماری ماژولار، طراحی Domain-Driven و پشتیبانی از چندفروشندگی.

---

## معرفی پروژه

این پروژه یک سیستم فروشگاه آنلاین کامل است که با **.NET 10** و **SQL Server** توسعه یافته است. طراحی سیستم بر اساس اصول Domain-Driven Design، الگوی CQRS و معماری ماژولار انجام شده است. سیستم شامل 12 ماژول مستقل با 27 جدول دیتابیس است.

### ویژگی‌های کلیدی
- 🏗️ معماری سه‌لایه (Framework → AppCore → Web)
- 📦 12 ماژول مستقل (Identity, Product, Pricing, Inventory, Order, Discount, Payment, Shipping, Accounting, Vendor, Settings, Customization)
- 🔄 الگوی CQRS با Wolverine
- 📊 رویکرد Append-Only برای موجودی و قیمت
- 🏪 پشتیبانی از چند فروشندگی (Multi-Vendor)
- 🚚 سیستم تحویل ماژولار (فیزیکی، حضوری، دیجیتال)
- 🛡️ حذف نرم (Soft Delete) و ثبت تغییرات (Audit)
- 🔐 هش رمز عبور با SHA256 + Salt

---

## پیش‌نیازها

- **.NET 10 SDK** (آخرین نسخه preview)
- **SQL Server** (2019 یا بالاتر)
- **Visual Studio 2022** یا **VS Code** (اختیاری)

---

## ساختار پروژه

```
ECommerce/
├── Framework/                      # لایه مشترک
│   ├── Exceptions/
│   │   └── GlobalExceptionHandler.cs
│   ├── DatatableModels/
│   │   └── DatatableModels.cs
│   ├── Security/
│   │   └── UserInfo.cs
│   ├── ResultHelper/
│   │   └── ResultOperation.cs
│   └── Framework.csproj
│
├── AppCore/                        # لایه هسته اپلیکیشن
│   ├── Data/
│   │   ├── ECommerceDbContext.cs
│   │   ├── Configurations/         # EF Core Fluent API
│   │   │   ├── Identity/
│   │   │   ├── Product/
│   │   │   ├── Pricing/
│   │   │   ├── Inventory/
│   │   │   ├── Order/
│   │   │   ├── Discount/
│   │   │   ├── Payment/
│   │   │   ├── Shipping/
│   │   │   ├── Accounting/
│   │   │   ├── Vendor/
│   │   │   ├── Settings/
│   │   │   └── Customization/
│   │   └── SeedData/
│   │       └── ECommerceSeedData.cs
│   ├── Domain/                     # موجودیت‌های دامنه
│   │   ├── Common/                 # BaseEntity, AuditableEntity, SoftDeletableEntity
│   │   ├── Identity/
│   │   ├── Product/
│   │   ├── Pricing/
│   │   ├── Inventory/
│   │   ├── Order/
│   │   ├── Discount/
│   │   ├── Payment/
│   │   ├── Shipping/
│   │   ├── Accounting/
│   │   ├── Vendor/
│   │   ├── Settings/
│   │   └── Customization/
│   ├── Features/                   # CQRS Handlers
│   │   ├── Identity/
│   │   ├── Product/
│   │   ├── Pricing/
│   │   ├── Inventory/
│   │   ├── Order/
│   │   ├── Discount/
│   │   ├── Payment/
│   │   ├── Shipping/
│   │   ├── Accounting/
│   │   ├── Vendor/
│   │   ├── Settings/
│   │   └── Customization/
│   ├── Enums/
│   └── AppCore.csproj
│
├── Web/                            # لایه ارائه
│   ├── Controllers/Admin/
│   │   ├── Identity/
│   │   ├── Product/
│   │   ├── Pricing/
│   │   ├── Inventory/
│   │   ├── Order/
│   │   ├── Discount/
│   │   ├── Payment/
│   │   ├── Shipping/
│   │   ├── Accounting/
│   │   ├── Vendor/
│   │   ├── Settings/
│   │   └── Customization/
│   ├── Helper/
│   │   └── ApiPathHelper.cs
│   ├── Program.cs
│   └── Web.csproj
│
├── Tests/                          # پروژه تست
│   ├── TestHelper.cs
│   ├── Features/
│   │   ├── Identity/
│   │   ├── Product/
│   │   ├── Order/
│   │   ├── Inventory/
│   │   └── Pricing/
│   └── Tests.csproj
│
├── docs/                           # مستندات
│   ├── architecture.md
│   ├── design-decisions.md
│   ├── services-guide.md
│   └── modules.md
│
└── database-script.sql             # اسکریپت دیتابیس
```

---

## نحوه راه‌اندازی

### 1. کلون پروژه
```bash
git clone <repository-url>
cd ECommerce
```

### 2. تنظیم Connection String
فایل `Web/appsettings.json` را ویرایش کنید:
```json
{
  "ConnectionStrings": {
    "ECommerceDb": "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. اجرای پروژه
```bash
cd Web
dotnet run
```

در اولین اجرا:
- ✅ مایگریشن‌ها به‌صورت خودکار اعمال می‌شوند
- ✅ داده‌های اولیه (Seed Data) به‌صورت خودکار ثبت می‌شوند شامل:
  - ارزها (تومان، دلار)
  - تنظیمات سیستم (15 مورد)
  - مقادیر شمارش (14 نوع)
  - درگاه پرداخت (زرین‌پال)
  - انبار مرکزی
  - حساب‌های حسابداری (6 حساب)
  - روش‌های ارسال (3 روش)
  - پوسته پیش‌فرض
  - اسلایدرها (2 مورد)
  - کاربر مدیر (admin / admin123)

### 4. آدرس‌های مهم
| آدرس | توضیح |
|------|-------|
| `https://localhost:5001/scalar/v1` | مستندات API (Scalar UI) |
| `https://localhost:5001/openapi/v1.json` | فایل OpenAPI |

---

## ساختار اسکیماها

دیتابیس شامل **12 اسکیما** است که هر ماژول اسکیما مستقل دارد:

| اسکیما | جداول | تعداد |
|--------|-------|-------|
| Identity | Users, Roles, UserRoles | 3 |
| Product | Products, ProductVariants, Categories, Brands, ProductCategories, ProductAttributes, ProductImages | 7 |
| Pricing | Prices, Currencies | 2 |
| Inventory | Warehouses, StockLedgers, StockReservations | 3 |
| Order | Orders, OrderItems, Carts, CartItems, OrderStatusHistories | 5 |
| Discount | Discounts, ProductDiscounts, CategoryDiscounts, OrderDiscounts | 4 |
| Payment | Payments, PaymentGateways | 2 |
| Shipping | Carriers, DeliveryMethods, OrderDeliveries | 3 |
| Accounting | Accounts, Transactions | 2 |
| Vendor | Vendors | 1 |
| Settings | Settings, EnumValues | 2 |
| Customization | SiteThemes, Sliders, Widgets | 3 |

**مجموع: 27 جدول**

---

## الگوهای طراحی استفاده شده

| الگو | کاربرد |
|------|--------|
| **CQRS** | تفکیک دستورات و کوئری‌ها با Wolverine |
| **Append-Only** | موجودی (StockLedger) و قیمت (Price) |
| **Soft Delete** | حذف نرم محصولات و تنوع‌ها |
| **Repository Pattern** | DbContext به‌عنوان Unit of Work |
| **Domain Model** | موجودیت‌های غنی با منطق کسب‌وکار |
| **Audit Pattern** | CreatedAt/CreatedBy/UpdatedAt/UpdatedBy خودکار |
| **Schema-per-Module** | اسکیما تفکیکی برای هر ماژول |
| **Snapshot Pattern** | ذخیره نام/قیمت در OrderItem |

---

## نحوه افزودن ماژول جدید

### 1. ایجاد موجودیت دامنه
```
AppCore/Domain/NewModule/NewEntity.cs
```

### 2. ایجاد Configuration
```
AppCore/Data/Configurations/NewModule/NewEntityConfiguration.cs
```
- `builder.ToTable("TableName", "NewModule");` → اسکیما جدید

### 3. افزودن DbSet
در `ECommerceDbContext.cs`:
```csharp
public DbSet<NewEntity> NewEntities { get; set; }
```

### 4. ایجاد Command و Query Handlers
```
AppCore/Features/NewModule/NewEntity/Commands/CreateXxxCommand.cs
AppCore/Features/NewModule/NewEntity/Queries/GetXxxByIdQuery.cs
```

### 5. ایجاد Controller
```
Web/Controllers/Admin/NewModule/NewEntityController.cs
```

### 6. افزودن مسیرها
در `ApiPathHelper.cs`:
```csharp
public class NewModule
{
    public const string BaseNewModule = $"{BaseAdmin}/new-module";
    // ...
}
```

---

## نحوه اجرای تست‌ها

```bash
# از پوشه ریشه پروژه
cd Tests
dotnet test

# با خروجی تفصیلی
dotnet test --verbosity detailed

# اجرای تست خاص
dotnet test --filter "FullyQualifiedName~UserCommandTests"
```

### تست‌های موجود

| فایل | تعداد تست | ماژول |
|------|-----------|-------|
| UserCommandTests.cs | 6 | Identity |
| ProductCommandTests.cs | 5 | Product |
| OrderCommandTests.cs | 4 | Order |
| StockCommandTests.cs | 7 | Inventory |
| PriceCommandTests.cs | 5 | Pricing |

تست‌ها از **In-Memory Database** استفاده می‌کنند و نیازی به SQL Server ندارند.

---

## اطلاعات ورود پیش‌فرض

| فیلد | مقدار |
|------|-------|
| نام کاربری | `admin` |
| رمز عبور | `admin123` |
| نقش | Admin (مدیر سیستم) |

⚠️ **حتماً رمز عبور را پس از اولین ورود تغییر دهید!**
