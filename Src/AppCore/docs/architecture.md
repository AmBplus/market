# معماری سیستم فروشگاه آنلاین ECommerce

## نمای کلی معماری

این پروژه بر اساس **معماری سه‌لایه** طراحی شده است که تفکیک مسئولیت‌ها را تضمین می‌کند:

```
┌─────────────────────────────────────────────────┐
│                    Web Layer                     │
│  (ASP.NET Core API Controllers + Scalar UI)     │
├─────────────────────────────────────────────────┤
│                   AppCore Layer                  │
│  (Domain Models + CQRS Handlers + EF Core)      │
├─────────────────────────────────────────────────┤
│                 Framework Layer                  │
│  (Shared Utilities + ResultOperation + etc.)    │
└─────────────────────────────────────────────────┘
```

### لایه Framework
- **ResultOperation**: کلاس‌های نتیجه عملیات (با و بدون داده) برای استانداردسازی پاسخ‌ها
- **DatatableModels**: مدل‌های درخواست/پاسخ جداول داده‌ای (سرویس‌ساید پجینگ)
- **Security**: کلاس UserInfo برای دسترسی به اطلاعات کاربر جاری
- **Exceptions**: مدیریت سراسری استثناها (GlobalExceptionHandler)

### لایه AppCore
- **Domain**: موجودیت‌های دامنه (27 جدول در 12 اسکیما)
- **Data**: DbContext، Configurations و Seed Data
- **Features**: هندلرهای CQRS تفکیک‌شده بر اساس ماژول
- **Enums**: شمارش‌های سیستمی
- **SeedData**: داده‌های اولیه سیستم

### لایه Web
- **Controllers**: کنترلرهای API تفکیک‌شده بر اساس ماژول (زیر پوشه Admin)
- **Helper**: ApiPathHelper برای مدیریت مسیرهای API
- **Program.cs**: تنظیمات Wolverine، EF Core، Middleware

---

## اسکیماها و جداول (12 اسکیما)

| اسکیما | جداول | توضیح |
|--------|-------|-------|
| **Identity** | Users, Roles, UserRoles | مدیریت هویت و دسترسی |
| **Product** | Products, ProductVariants, Categories, Brands, ProductCategories, ProductAttributes, ProductImages | مدیریت کاتالوگ محصولات |
| **Pricing** | Prices, Currencies | مدیریت قیمت‌گذاری و ارز |
| **Inventory** | Warehouses, StockLedgers, StockReservations | مدیریت انبار و موجودی |
| **Order** | Orders, OrderItems, Carts, CartItems, OrderStatusHistories | مدیریت سفارشات |
| **Discount** | Discounts, ProductDiscounts, CategoryDiscounts, OrderDiscounts | مدیریت تخفیف‌ها |
| **Payment** | Payments, PaymentGateways | مدیریت پرداخت |
| **Shipping** | Carriers, DeliveryMethods, OrderDeliveries | مدیریت ارسال و تحویل |
| **Accounting** | Accounts, Transactions | مدیریت حسابداری |
| **Vendor** | Vendors | مدیریت فروشندگان |
| **Settings** | Settings, EnumValues | تنظیمات سیستم |
| **Customization** | SiteThemes, Sliders, Widgets | شخصی‌سازی ظاهری |

---

## دیاگرام روابط اصلی

```
Identity.Users ─────────┬── Order.Orders
                        ├── Order.Carts
                        └── Vendor.Vendors

Product.Products ───────┬── Product.ProductVariants
                        ├── Product.ProductCategories ──── Product.Categories
                        ├── Product.ProductAttributes
                        ├── Product.ProductImages
                        └── Product.Brands (FK)

Product.ProductVariants ─┬── Pricing.Prices
                         ├── Inventory.StockLedgers
                         ├── Inventory.StockReservations
                         ├── Order.CartItems
                         ├── Order.OrderItems
                         └── Discount.ProductDiscounts

Pricing.Currencies ─────┬── Pricing.Prices
                        ├── Order.Orders
                        ├── Order.CartItems
                        ├── Payment.Payments
                        ├── Shipping.DeliveryMethods
                        └── Accounting.Transactions

Inventory.Warehouses ───┬── Inventory.StockLedgers
                        └── Inventory.StockReservations

Order.Orders ────────────┬── Order.OrderItems
                        ├── Order.OrderStatusHistories
                        ├── Payment.Payments
                        ├── Shipping.OrderDeliveries
                        ├── Discount.OrderDiscounts
                        └── Inventory.StockReservations

Discount.Discounts ──────┬── Discount.ProductDiscounts
                        ├── Discount.CategoryDiscounts
                        └── Discount.OrderDiscounts

Accounting.Accounts ─────┬── Accounting.Transactions
                        └── Accounting.Accounts (Self-ref: ParentId)

Shipping.Carriers ───────└── Shipping.DeliveryMethods
```

---

## رویکرد Append-Only برای موجودی و قیمت

### موجودی (StockLedger)
سیستم موجودی بر اساس **Append-Only** طراحی شده است. به‌جای بروزرسانی رکورد موجودی، هر تغییری به‌صورت یک رکورد جدید در جدول `StockLedgers` ثبت می‌شود:

```
┌──────────────────────────────────────────────────────┐
│  StockLedger                                         │
│  ─────────────────────────────────────────────────── │
│  ProductVariantId | WarehouseId | QuantityChange     │
│  1                | 1           | +100  (ورود کالا)  │
│  1                | 1           | -5    (فروش)       │
│  1                | 1           | +3     (مرجوعی)    │
│  ─────────────────────────────────────────────────── │
│  موجودی فعلی = SUM(QuantityChange) = 98              │
└──────────────────────────────────────────────────────┘
```

**مزایا:**
- تاریخچه کامل تغییرات موجودی بدون از دست دادن اطلاعات
- امکان حسابرسی و ردیابی
- جلوگیری از Race Condition در بروزرسانی همزمان
- پشتیبانی از تحلیل‌های آماری

### قیمت (Price)
سیستم قیمت‌گذاری نیز Append-Only است. هر قیمت جدید با `StartedAt` ثبت می‌شود و قیمت قبلی با تنظیم `EndedAt` بسته می‌شود:

```
┌───────────────────────────────────────────────────────────────┐
│  Price                                                         │
│  ─────────────────────────────────────────────────────────────│
│  VariantId | Type    | Amount      | StartedAt  | EndedAt     │
│  1         | Retail  | 10,000,000  | 2024-01-01 | 2024-03-01  │
│  1         | Retail  | 12,000,000  | 2024-03-01 | NULL        │
│  ─────────────────────────────────────────────────────────────│
│  قیمت فعال فعلی: 12,000,000 (EndedAt IS NULL)                 │
└───────────────────────────────────────────────────────────────┘
```

**مزایا:**
- تاریخچه کامل تغییرات قیمت
- امکان بازگشت به قیمت قبلی
- تحلیل روند قیمت

---

## سیستم رزرو موجودی

هنگام ثبت سفارش، موجودی کالاها **رزرو** می‌شود تا از فروش بیش از موجودی جلوگیری شود:

```
سبد خرید ← ثبت سفارش ← رزرو موجودی (StockReservation)
                           ├── StockLedger: QuantityChange = -quantity
                           └── StockReservation: ExpiresAt = 3 روز

لغو سفارش ← آزادسازی رزرو
               ├── StockReservation.IsReleased = true
               └── StockLedger: QuantityChange = +quantity (بازگشت)
```

**ویژگی‌ها:**
- هر رزرو تاریخ انقضا دارد (3 روز)
- در صورت لغو سفارش، رزرو آزاد می‌شود
- موجودی قابل دسترس = SUM(StockLedger) - SUM(رزروهای فعال)

---

## تفکیک Product و ProductVariant

محصولات به دو سطح تفکیک شده‌اند:

```
Product (محصول پایه)
├── نام، توضیحات، اسلاگ، برند
├── دسته‌بندی‌ها (چند به چند)
├── ویژگی‌ها (مشخصات فنی)
├── تصاویر
│
└── ProductVariant (تنوع محصول)
    ├── کد تنوع، SKU، عنوان
    ├── ویژگی‌های تنوع (JSON): {"رنگ":"مشکی","سایز":"XL"}
    ├── قیمت پیش‌فرض و واحد پول
    ├── وزن
    ├── فروشنده (اختیاری)
    └── قیمت‌ها، موجودی
```

**مثال:**
- Product: "آیفون 15 پرو"
  - Variant 1: "مشکی - 128 گیگ" → SKU: IP15P-BLK-128
  - Variant 2: "سفید - 256 گیگ" → SKU: IP15P-WHT-256
  - Variant 3: "آبی - 512 گیگ" → SKU: IP15P-BLU-512

---

## سیستم چند فروشندگی (Multi-Vendor)

هر تنوع محصول می‌تواند به یک فروشنده اختصاص یابد. کمیسیون فروشنده در سطح Vendor و OrderItem ثبت می‌شود:

```
Vendor
├── StoreName, StoreSlug
├── CommissionRate (DECIMAL(5,2))
├── Status: Pending → Active → Suspended / Rejected
├── BankAccountNumber
│
├── Products (محصولات متعلق به فروشنده)
└── ProductVariants (تنوع‌های متعلق به فروشنده)

OrderItem
├── VendorId (فروشنده)
└── VendorCommissionRate (نرخ کمیسیون در زمان سفارش)
```

**فرآیند:**
1. فروشنده ثبت‌نام می‌کند (Status = Pending)
2. مدیر تأیید می‌کند (Status = Active)
3. محصول/تنوع به فروشنده اختصاص می‌یابد
4. در زمان سفارش، نرخ کمیسیون ثبت می‌شود (برای جلوگیری از تغییر بعدي)

---

## سیستم تحویل ماژولار

تحویل سفارشات بر اساس نوع (DeliveryType) ماژولار طراحی شده است:

| DeliveryType | توضیح | محتوا |
|---|---|---|
| **Physical** | ارسال پستی/حضوری | آدرس فیزیکی + کد رهگیری |
| **InPerson** | تحویل حضوری | آدرس فروشگاه |
| **Digital** | محتوای دیجیتال | لینک/فایل/محتوا |

```
DeliveryMethod
├── DeliveryType (Physical/InPerson/Digital)
├── Carrier (حامل بار - اختیاری)
├── Price, CurrencyId
├── ConstraintsJson (محدودیت‌های ارسال)
└── DigitalContentType (Link/File/Content)

OrderDelivery
├── DeliveryMethodId
├── Status (Pending → InTransit → Delivered / Failed)
├── TrackingCode (کد رهگیری فیزیکی)
├── DigitalContent (محتوای دیجیتال)
└── DeliveryAddressJson (آدرس تحویل)
```

---

## الگوی CQRS با Wolverine

سیستم از الگوی **CQRS** (Command Query Responsibility Segregation) با کتابخانه **Wolverine** استفاده می‌کند:

### ساختار پوشه‌ها
```
Features/
└── [Module]/
    ├── [Entity]/
    │   ├── Commands/
    │   │   ├── CreateXxxCommand.cs  (Command + Handler)
    │   │   ├── UpdateXxxCommand.cs
    │   │   └── DeleteXxxCommand.cs
    │   └── Queries/
    │       ├── GetXxxByIdQuery.cs   (Query + Handler)
    │       ├── GetXxxDataTableQuery.cs
    │       └── SelectXxxQuery.cs
```

### جریان پردازش
```
Controller → Wolverine Bus → Handler → DbContext → ResultOperation
```

- **Commands**: تغییرات داده (Create, Update, Delete) → خروجی `ResultOperation` یا `ResultOperation<T>`
- **Queries**: خواندن داده (GetById, GetAll, DataTable) → خروجی `ResultOperation<T>`

### مزایای Wolverine
- رزولو خودکار هندلرها بر اساس نوع پیام
- پشتیبانی از middleware برای اعتبارسنجی، لاگ و...
- امکان آسان ارتقا به پردازش ناهمزمان (message queue)
- تست‌پذیری بالا (هندلرها کلاس‌های ساده هستند)
