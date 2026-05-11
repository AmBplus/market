# مستندات ماژول‌های سیستم فروشگاه ECommerce

## 1. ماژول Identity (هویت)

### موجودیت‌ها

#### User (کاربر)
| فیلد | نوع | توضیح |
|------|------|-------|
| Id | BIGINT | شناسه یکتا |
| UserName | NVARCHAR(100) | نام کاربری (یکتا) |
| Email | NVARCHAR(200)? | ایمیل (یکتا) |
| PhoneNumber | NVARCHAR(20)? | شماره تلفن (یکتا) |
| PasswordHash | NVARCHAR(500) | هش رمز عبور (SHA256) |
| PasswordSalt | NVARCHAR(500) | نمک رمز عبور |
| FirstName | NVARCHAR(100) | نام |
| LastName | NVARCHAR(100) | نام خانوادگی |
| NationalCode | NVARCHAR(10)? | کد ملی (یکتا) |
| DefaultAddressesJson | NVARCHAR(MAX)? | آدرس‌های پیش‌فرض (JSON) |
| IsActive | BIT | فعال/غیرفعال |
| IsEmailConfirmed | BIT | تأیید ایمیل |
| IsPhoneNumberConfirmed | BIT | تأیید شماره تلفن |
| LastLoginAt | DATETIME2? | آخرین ورود |
| ResetPasswordToken | NVARCHAR? | توکن بازنشانی رمز |
| ResetPasswordTokenExpireAt | DATETIME2? | انقضای توکن |

**روابط:** User → UserRole (یک به چند)، User → Order (یک به چند)، User → Cart (یک به چند)

#### Role (نقش)
| فیلد | نوع | توضیح |
|------|------|-------|
| Id | BIGINT | شناسه یکتا |
| Name | NVARCHAR(100) | نام نقش (یکتا) |
| DisplayName | NVARCHAR(100) | نام نمایشی |
| Description | NVARCHAR(500)? | توضیحات |
| IsActive | BIT | فعال/غیرفعال |

**روابط:** Role → UserRole (یک به چند)

#### UserRole (نقش کاربر)
| فیلد | نوع | توضیح |
|------|------|-------|
| Id | BIGINT | شناسه یکتا |
| UserId | BIGINT | شناسه کاربر (FK) |
| RoleId | BIGINT | شناسه نقش (FK) |
| AssignedAt | DATETIME2 | زمان اختصاص |

**یکتایی:** (UserId, RoleId)

---

## 2. ماژول Product (محصول)

### Product (محصول پایه)
محصول پایه اطلاعات کلی محصول را نگه می‌دارد. هر محصول می‌تواند چندین تنوع (Variant) داشته باشد.

| فیلد | نوع | توضیح |
|------|------|-------|
| ProductCode | NVARCHAR(50) | کد محصول (یکتا) |
| Name | NVARCHAR(300) | نام محصول |
| Slug | NVARCHAR(300) | اسلاگ URL (یکتا) |
| Description | NVARCHAR(MAX)? | توضیحات کامل |
| ShortDescription | NVARCHAR(500)? | توضیحات کوتاه |
| BrandId | BIGINT? | برند (FK) |
| VendorId | BIGINT? | فروشنده (FK) |
| ShippingConstraintsJson | NVARCHAR(MAX)? | محدودیت‌های ارسال |
| MetaTitle | NVARCHAR(200)? | عنوان سئو |
| MetaDescription | NVARCHAR(500)? | توضیحات سئو |
| IsActive | BIT | فعال |
| IsFeatured | BIT | ویژه |
| IsDeleted | BIT | حذف نرم |

**روابط:** Product → ProductVariant (یک به چند)، Product → ProductCategory (چند به چند با Category)، Product → ProductAttribute (یک به چند)، Product → ProductImage (یک به چند)

### ProductVariant (تنوع محصول)
هر تنوع نمایانگر یک نسخه مشخص از محصول است (مثلاً رنگ/سایز متفاوت).

| فیلد | نوع | توضیح |
|------|------|-------|
| ProductId | BIGINT | محصول والد (FK) |
| VariantCode | NVARCHAR(50) | کد تنوع (یکتا) |
| Sku | NVARCHAR(50) | SKU (یکتا) |
| Title | NVARCHAR(300) | عنوان تنوع |
| VariantAttributesJson | NVARCHAR(MAX)? | ویژگی‌های تنوع (JSON) |
| VendorId | BIGINT? | فروشنده (FK) |
| CostPrice | DECIMAL(18,2)? | قیمت تمام‌شده |
| DefaultPrice | DECIMAL(18,2) | قیمت پیش‌فرض |
| CurrencyId | BIGINT | واحد پول (FK) |
| WeightGrams | DECIMAL(10,2)? | وزن (گرم) |
| IsActive | BIT | فعال |
| IsDefault | BIT | تنوع پیش‌فرض |
| IsDeleted | BIT | حذف نرم |

**روابط:** ProductVariant → Price (یک به چند)، ProductVariant → StockLedger (یک به چند)، ProductVariant → StockReservation (یک به چند)، ProductVariant → CartItem (یک به چند)، ProductVariant → OrderItem (یک به چند)

### Category (دسته‌بندی)
| فیلد | نوع | توضیح |
|------|------|-------|
| ParentId | BIGINT? | دسته‌بندی والد (FK خودارجاع) |
| Code | NVARCHAR(50) | کد (یکتا) |
| Name | NVARCHAR(200) | نام |
| Slug | NVARCHAR(200) | اسلاگ (یکتا) |
| Description | NVARCHAR(2000)? | توضیحات |
| ImageUrl | NVARCHAR(500)? | تصویر |
| DisplayOrder | INT | ترتیب نمایش |
| IsActive | BIT | فعال |

### Brand (برند)
| فیلد | نوع | توضیح |
|------|------|-------|
| Name | NVARCHAR(200) | نام (یکتا) |
| Slug | NVARCHAR(200) | اسلاگ (یکتا) |
| LogoUrl | NVARCHAR(500)? | لوگو |
| Description | NVARCHAR(2000)? | توضیحات |
| IsActive | BIT | فعال |

### ProductCategory (رابطه محصول-دسته‌بندی)
| فیلد | نوع | توضیح |
|------|------|-------|
| ProductId | BIGINT | محصول (FK) |
| CategoryId | BIGINT | دسته‌بندی (FK) |
| DisplayOrder | INT | ترتیب |

**یکتایی:** (ProductId, CategoryId)

### ProductAttribute (ویژگی محصول)
| فیلد | نوع | توضیح |
|------|------|-------|
| ProductId | BIGINT | محصول (FK) |
| Key | NVARCHAR(200) | کلید ویژگی |
| Value | NVARCHAR(500) | مقدار ویژگی |
| Group | NVARCHAR(100)? | گروه ویژگی |
| DisplayOrder | INT | ترتیب |

### ProductImage (تصویر محصول)
| فیلد | نوع | توضیح |
|------|------|-------|
| ProductId | BIGINT | محصول (FK) |
| ProductVariantId | BIGINT? | تنوع (FK اختیاری) |
| ImageUrl | NVARCHAR(500) | مسیر تصویر |
| AltText | NVARCHAR(200)? | متن جایگزین |
| DisplayOrder | INT | ترتیب |
| IsPrimary | BIT | تصویر اصلی |

---

## 3. ماژول Pricing (قیمت‌گذاری)

### Currency (واحد پول)
| فیلد | نوع | توضیح |
|------|------|-------|
| Code | NVARCHAR(10) | کد ارز (یکتا) |
| Name | NVARCHAR(100) | نام |
| Symbol | NVARCHAR(10) | نماد |
| ExchangeRate | DECIMAL(18,6) | نرخ تبدیل به ارز پایه |
| IsBase | BIT | ارز پایه |
| IsActive | BIT | فعال |

### Price (قیمت)
قیمت‌ها به‌صورت Append-Only ذخیره می‌شوند. قیمت فعال: `EndedAt IS NULL`.

| فیلد | نوع | توضیح |
|------|------|-------|
| ProductVariantId | BIGINT | تنوع محصول (FK) |
| Amount | DECIMAL(18,2) | مبلغ |
| CurrencyId | BIGINT | واحد پول (FK) |
| PriceType | TINYINT | نوع قیمت (Retail/Wholesale/Special/Cost) |
| StartedAt | DATETIME2 | تاریخ شروع اعتبار |
| EndedAt | DATETIME2? | تاریخ پایان اعتبار (NULL=فعال) |
| MinQuantity | INT? | حداقل تعداد (برای قیمت عمده) |

---

## 4. ماژول Inventory (انبار و موجودی)

### Warehouse (انبار)
| فیلد | نوع | توضیح |
|------|------|-------|
| Name | NVARCHAR(200) | نام انبار |
| Code | NVARCHAR(50) | کد انبار (یکتا) |
| Address | NVARCHAR(500)? | آدرس |
| City | NVARCHAR(100)? | شهر |
| Province | NVARCHAR(100)? | استان |
| IsActive | BIT | فعال |

### StockLedger (دفتر موجودی)
هر رکورد نمایانگر یک تغییر موجودی است. موجودی فعلی = SUM(QuantityChange).

| فیلد | نوع | توضیح |
|------|------|-------|
| ProductVariantId | BIGINT | تنوع محصول (FK) |
| WarehouseId | BIGINT | انبار (FK) |
| QuantityChange | INT | تغییر مقدار (مثبت/منفی) |
| Reason | TINYINT | دلیل تغییر (StockIn/StockOut/Return/Damaged/Initial/ManualAdjustment/Reserved/ReservationReleased) |
| ReferenceId | BIGINT? | شناسه مرجع (سفارش و...) |
| Description | NVARCHAR(500)? | توضیحات |

### StockReservation (رزرو موجودی)
رزرو موقت موجودی هنگام ثبت سفارش.

| فیلد | نوع | توضیح |
|------|------|-------|
| ProductVariantId | BIGINT | تنوع محصول (FK) |
| WarehouseId | BIGINT | انبار (FK) |
| OrderId | BIGINT | سفارش (FK) |
| Quantity | INT | تعداد رزرو |
| ExpiresAt | DATETIME2 | تاریخ انقضا |
| IsReleased | BIT | آزاد شده |
| ReleasedAt | DATETIME2? | زمان آزادسازی |

---

## 5. ماژول Order (سفارش)

### Cart (سبد خرید)
| فیلد | نوع | توضیح |
|------|------|-------|
| UserId | BIGINT | کاربر (FK) |
| DiscountCode | NVARCHAR(50)? | کد تخفیف |
| IsActive | BIT | فعال |

### CartItem (آیتم سبد)
| فیلد | نوع | توضیح |
|------|------|-------|
| CartId | BIGINT | سبد (FK) |
| ProductVariantId | BIGINT | تنوع (FK) |
| Quantity | INT | تعداد |
| UnitPrice | DECIMAL(18,2) | قیمت واحد |
| CurrencyId | BIGINT | واحد پول (FK) |

**یکتایی:** (CartId, ProductVariantId)

### Order (سفارش)
| فیلد | نوع | توضیح |
|------|------|-------|
| OrderNumber | NVARCHAR(50) | شماره سفارش (یکتا) |
| UserId | BIGINT | کاربر (FK) |
| Status | TINYINT | وضعیت (Pending/Paid/Processing/Shipped/Delivered/Cancelled/Refunded) |
| SubTotal | DECIMAL(18,2) | جمع کل |
| DiscountAmount | DECIMAL(18,2) | تخفیف |
| ShippingCost | DECIMAL(18,2) | هزینه ارسال |
| TaxAmount | DECIMAL(18,2) | مالیات |
| TotalAmount | DECIMAL(18,2) | مبلغ نهایی |
| CurrencyId | BIGINT | واحد پول (FK) |
| CurrencyRate | DECIMAL(18,6) | نرخ تبدیل |
| CustomerNote | NVARCHAR(500)? | یادداشت مشتری |
| AdminNote | NVARCHAR(500)? | یادداشت مدیر |
| ShippingAddressJson | NVARCHAR(MAX) | آدرس ارسال |
| CompletedAt | DATETIME2? | زمان تکمیل |

### OrderItem (آیتم سفارش)
| فیلد | نوع | توضیح |
|------|------|-------|
| OrderId | BIGINT | سفارش (FK) |
| ProductVariantId | BIGINT | تنوع محصول (FK) |
| ProductName | NVARCHAR(300) | نام محصول (snapshot) |
| VariantTitle | NVARCHAR(300) | عنوان تنوع (snapshot) |
| Quantity | INT | تعداد |
| UnitPrice | DECIMAL(18,2) | قیمت واحد |
| DiscountAmount | DECIMAL(18,2) | تخفیف |
| TotalPrice | DECIMAL(18,2) | مبلغ کل |
| VendorId | BIGINT? | فروشنده (FK) |
| VendorCommissionRate | DECIMAL(5,2)? | نرخ کمیسیون |

### OrderStatusHistory (تاریخچه وضعیت)
| فیلد | نوع | توضیح |
|------|------|-------|
| OrderId | BIGINT | سفارش (FK) |
| FromStatus | TINYINT | وضعیت قبلی |
| ToStatus | TINYINT | وضعیت جدید |
| Note | NVARCHAR(500)? | یادداشت |
| ChangedBy | BIGINT? | تغییردهنده |
| ChangedAt | DATETIME2 | زمان تغییر |

---

## 6. ماژول Discount (تخفیف)

### Discount (تخفیف)
| فیلد | نوع | توضیح |
|------|------|-------|
| Code | NVARCHAR(50) | کد تخفیف (یکتا) |
| Name | NVARCHAR(200) | نام |
| Description | NVARCHAR(1000)? | توضیحات |
| DiscountType | TINYINT | نوع (Percentage/FixedAmount/FreeShipping) |
| Scope | TINYINT | محدوده (Order/Product/Category/Shipping) |
| Value | DECIMAL(18,2) | مقدار |
| MaxDiscountAmount | DECIMAL(18,2)? | حداکثر تخفیف |
| MinOrderAmount | DECIMAL(18,2)? | حداقل سفارش |
| UsageLimit | INT? | سقف استفاده |
| UsedCount | INT | تعداد استفاده |
| PerUserLimit | INT? | سقف هر کاربر |
| StartedAt | DATETIME2 | شروع |
| EndedAt | DATETIME2? | پایان |
| IsActive | BIT | فعال |

### ProductDiscount, CategoryDiscount, OrderDiscount
جداول رابطه تخفیف با محصول، دسته‌بندی و سفارش.

---

## 7. ماژول Payment (پرداخت)

### PaymentGateway (درگاه پرداخت)
| فیلد | نوع | توضیح |
|------|------|-------|
| Name | NVARCHAR(200) | نام درگاه |
| Code | NVARCHAR(50) | کد (یکتا) |
| ConfigJson | NVARCHAR(MAX) | تنظیمات (JSON) |
| Priority | INT | اولویت |
| IsActive | BIT | فعال |

### Payment (پرداخت)
| فیلد | نوع | توضیح |
|------|------|-------|
| OrderId | BIGINT | سفارش (FK) |
| GatewayId | BIGINT | درگاه (FK) |
| Amount | DECIMAL(18,2) | مبلغ |
| CurrencyId | BIGINT | واحد پول (FK) |
| Status | TINYINT | وضعیت (Created/Pending/Succeeded/Failed/Refunded) |
| TransactionRef | NVARCHAR(100)? | شماره تراکنکشن بانک |
| TrackingCode | NVARCHAR(100)? | کد پیگیری |
| PaymentToken | NVARCHAR(500)? | توکن پرداخت |
| CallbackUrl | NVARCHAR(500)? | آدرس بازگشت |
| BankMessage | NVARCHAR(500)? | پیام بانک |
| PaidAt | DATETIME2? | زمان پرداخت |

---

## 8. ماژول Shipping (ارسال و تحویل)

### Carrier (حامل بار)
| فیلد | نوع | توضیح |
|------|------|-------|
| Name | NVARCHAR(200) | نام شرکت |
| Code | NVARCHAR(50) | کد (یکتا) |
| TrackingUrlTemplate | NVARCHAR(500)? | الگوی URL رهگیری |
| ConfigJson | NVARCHAR(MAX)? | تنظیمات |
| IsActive | BIT | فعال |

### DeliveryMethod (روش ارسال)
| فیلد | نوع | توضیح |
|------|------|-------|
| Name | NVARCHAR(200) | نام روش |
| DeliveryType | TINYINT | نوع (Physical/InPerson/Digital) |
| CarrierId | BIGINT? | حامل (FK) |
| Price | DECIMAL(18,2) | هزینه |
| CurrencyId | BIGINT | واحد پول (FK) |
| EstimatedDays | INT? | روز تخمینی |
| Description | NVARCHAR(500)? | توضیحات |
| ConstraintsJson | NVARCHAR(MAX)? | محدودیت‌ها |
| DigitalContentType | TINYINT? | نوع محتوای دیجیتال |
| IsActive | BIT | فعال |

### OrderDelivery (تحویل سفارش)
| فیلد | نوع | توضیح |
|------|------|-------|
| OrderId | BIGINT | سفارش (FK) |
| DeliveryMethodId | BIGINT | روش ارسال (FK) |
| Status | TINYINT | وضعیت (Pending/InTransit/Delivered/Failed) |
| DeliveryAddressJson | NVARCHAR(MAX)? | آدرس (JSON) |
| TrackingCode | NVARCHAR(100)? | کد رهگیری |
| DigitalContent | NVARCHAR(MAX)? | محتوای دیجیتال |
| DigitalContentType | TINYINT? | نوع محتوا |
| ShippedAt | DATETIME2? | زمان ارسال |
| DeliveredAt | DATETIME2? | زمان تحویل |
| Note | NVARCHAR(500)? | یادداشت |

---

## 9. ماژول Accounting (حسابداری)

### Account (حساب)
| فیلد | نوع | توضیح |
|------|------|-------|
| Code | NVARCHAR(20) | کد حساب (یکتا) |
| Name | NVARCHAR(200) | نام |
| AccountType | TINYINT | نوع (Asset/Liability/Revenue/Expense/Equity) |
| ParentId | BIGINT? | حساب والد (FK خودارجاع) |
| Description | NVARCHAR(500)? | توضیحات |
| IsActive | BIT | فعال |

### Transaction (تراکنش)
| فیلد | نوع | توضیح |
|------|------|-------|
| AccountId | BIGINT | حساب (FK) |
| BatchId | UNIQUEIDENTIFIER | شناسه دسته |
| TransactionType | TINYINT | نوع (Debit/Credit) |
| Amount | DECIMAL(18,2) | مبلغ |
| CurrencyId | BIGINT | واحد پول (FK) |
| Source | TINYINT | منبع (Order/Refund/Shipping/VendorCommission/Operational/Manual) |
| ReferenceId | BIGINT? | شناسه مرجع |
| Description | NVARCHAR(500)? | توضیحات |

---

## 10. ماژول Vendor (فروشنده)

### Vendor
| فیلد | نوع | توضیح |
|------|------|-------|
| UserId | BIGINT | کاربر (FK, یکتا) |
| StoreName | NVARCHAR(200) | نام فروشگاه |
| StoreSlug | NVARCHAR(200) | اسلاگ (یکتا) |
| Description | NVARCHAR(1000)? | توضیحات |
| LogoUrl | NVARCHAR(500)? | لوگو |
| CommissionRate | DECIMAL(5,2) | نرخ کمیسیون |
| Status | TINYINT | وضعیت (Pending/Active/Suspended/Rejected) |
| BankAccountNumber | NVARCHAR(30)? | شماره حساب بانکی |
| IsActive | BIT | فعال |

---

## 11. ماژول Settings (تنظیمات)

### Setting
| فیلد | نوع | توضیح |
|------|------|-------|
| Key | NVARCHAR(200) | کلید |
| Value | NVARCHAR(MAX) | مقدار |
| Module | NVARCHAR(100) | ماژول |
| Description | NVARCHAR(500)? | توضیحات |
| ValueType | NVARCHAR(50) | نوع مقدار (String/Int/Decimal/Boolean/Long) |
| IsEditable | BIT | قابل ویرایش |

**یکتایی:** (Key, Module)

### EnumValue
| فیلد | نوع | توضیح |
|------|------|-------|
| EnumType | NVARCHAR(100) | نوع شمارش |
| EnumKey | NVARCHAR(100) | کلید |
| EnumId | INT | شناسه عددی |
| Name | NVARCHAR(200) | نام نمایشی |
| Description | NVARCHAR(500)? | توضیحات |
| DisplayOrder | INT | ترتیب |
| IsActive | BIT | فعال |

**یکتایی:** (EnumType, EnumKey)

---

## 12. ماژول Customization (شخصی‌سازی)

### SiteTheme
| فیلد | نوع | توضیح |
|------|------|-------|
| Name | NVARCHAR(200) | نام پوسته |
| PrimaryColor | NVARCHAR(7) | رنگ اصلی |
| SecondaryColor | NVARCHAR(7) | رنگ ثانویه |
| BackgroundColor | NVARCHAR(7) | رنگ پس‌زمینه |
| TextColor | NVARCHAR(7) | رنگ متن |
| AccentColor | NVARCHAR(7) | رنگ تأکیدی |
| FontFamily | NVARCHAR(100)? | فونت |
| ExtendedSettingsJson | NVARCHAR(MAX)? | تنظیمات اضافی |
| IsActive | BIT | فعال |

### Slider
| فیلد | نوع | توضیح |
|------|------|-------|
| Title | NVARCHAR(200) | عنوان |
| SubTitle | NVARCHAR(300)? | زیرعنوان |
| ImageUrl | NVARCHAR(500) | تصویر |
| MobileImageUrl | NVARCHAR(500)? | تصویر موبایل |
| LinkUrl | NVARCHAR(500)? | لینک |
| LinkText | NVARCHAR(100)? | متن لینک |
| DisplayOrder | INT | ترتیب |
| IsActive | BIT | فعال |
| StartedAt | DATETIME2? | شروع نمایش |
| EndedAt | DATETIME2? | پایان نمایش |

### Widget
| فیلد | نوع | توضیح |
|------|------|-------|
| Type | NVARCHAR(100) | نوع ویجت |
| Title | NVARCHAR(200) | عنوان |
| ConfigJson | NVARCHAR(MAX) | تنظیمات |
| Position | NVARCHAR(100) | موقعیت |
| DisplayOrder | INT | ترتیب |
| IsActive | BIT | فعال |
| StartedAt | DATETIME2? | شروع |
| EndedAt | DATETIME2? | پایان |
