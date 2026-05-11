# راهنمای سرویس‌های سیستم فروشگاه ECommerce

## فهرست ماژول‌ها و سرویس‌ها

---

## 1. ماژول Identity (هویت)

### 1.1 کاربران (Users)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد کاربر | `/api/admin/identity/user/create` | POST | `CreateUserCommand` | `ResultOperation` | ایجاد کاربر جدید با هش رمز عبور و اختصاص نقش |
| 2 | ویرایش کاربر | `/api/admin/identity/user/update` | PUT | `UpdateUserCommand` | `ResultOperation` | ویرایش اطلاعات کاربر (بدون تغییر رمز) |
| 3 | حذف کاربر | `/api/admin/identity/user/delete` | DELETE | `{Id, DeletedBy}` | `ResultOperation` | حذف نرم کاربر (Soft Delete) |
| 4 | تغییر رمز عبور | `/api/admin/identity/user/change-password` | POST | `ChangePasswordCommand` | `ResultOperation` | تغییر رمز عبور کاربر |
| 5 | دریافت با شناسه | `/api/admin/identity/user/get-by-id` | GET | `{id}` | `ResultOperation<User>` | دریافت اطلاعات کاربر بر اساس شناسه |
| 6 | لیست DataTable | `/api/admin/identity/user/get-all` | POST | `DataTableRequest` | `DataTableResponse<User>` | لیست کاربران با صفحه‌بندی و جستجو |
| 7 | Select2 | `/api/admin/identity/user/select2` | GET | `{search}` | `List<Select2Item>` | لیست کاربران برای دراپ‌داون |

**CreateUserCommand:**
```json
{
  "userName": "string",
  "email": "string?",
  "phoneNumber": "string?",
  "firstName": "string",
  "lastName": "string",
  "password": "string",
  "nationalCode": "string?",
  "roleIds": [1, 2]
}
```

**UpdateUserCommand:**
```json
{
  "id": 1,
  "email": "string?",
  "phoneNumber": "string?",
  "firstName": "string",
  "lastName": "string",
  "nationalCode": "string?",
  "isActive": true
}
```

### 1.2 نقش‌ها (Roles)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد نقش | `/api/admin/identity/role/create` | POST | `CreateRoleCommand` | `ResultOperation` | ایجاد نقش جدید |
| 2 | ویرایش نقش | `/api/admin/identity/role/update` | PUT | `UpdateRoleCommand` | `ResultOperation` | ویرایش اطلاعات نقش |
| 3 | حذف نقش | `/api/admin/identity/role/delete` | DELETE | `{Id}` | `ResultOperation` | حذف نقش و ارتباطات آن |
| 4 | دریافت با شناسه | `/api/admin/identity/role/get-by-id` | GET | `{id}` | `ResultOperation<Role>` | دریافت اطلاعات نقش |
| 5 | لیست DataTable | `/api/admin/identity/role/get-all` | POST | `DataTableRequest` | `DataTableResponse<Role>` | لیست نقش‌ها با صفحه‌بندی |
| 6 | Select2 | `/api/admin/identity/role/select2` | GET | `{search}` | `List<Select2Item>` | لیست نقش‌ها برای دراپ‌داون |

---

## 2. ماژول Product (محصول)

### 2.1 محصولات (Products)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد محصول | `/api/admin/product/products/create` | POST | `CreateProductCommand` | `ResultOperation` | ایجاد محصول با دسته‌بندی و ویژگی |
| 2 | ویرایش محصول | `/api/admin/product/products/update` | PUT | `UpdateProductCommand` | `ResultOperation` | ویرایش محصول (جایگزینی دسته‌بندی و ویژگی) |
| 3 | حذف محصول | `/api/admin/product/products/delete` | DELETE | `{Id}` | `ResultOperation` | حذف نرم (فقط بدون تنوع فعال) |
| 4 | دریافت با شناسه | `/api/admin/product/products/get-by-id` | GET | `{id}` | `ResultOperation<Product>` | دریافت اطلاعات محصول |
| 5 | لیست DataTable | `/api/admin/product/products/get-all` | POST | `DataTableRequest` | `DataTableResponse<Product>` | لیست محصولات با صفحه‌بندی |
| 6 | Select2 | `/api/admin/product/products/select2` | GET | `{search}` | `List<Select2Item>` | لیست محصولات برای دراپ‌داون |

### 2.2 تنوع محصول (ProductVariants)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد تنوع | `/api/admin/product/product-variants/create` | POST | `CreateProductVariantCommand` | `ResultOperation` | ایجاد تنوع جدید برای محصول |
| 2 | ویرایش تنوع | `/api/admin/product/product-variants/update` | PUT | `UpdateProductVariantCommand` | `ResultOperation` | ویرایش تنوع محصول |
| 3 | حذف تنوع | `/api/admin/product/product-variants/delete` | DELETE | `{Id}` | `ResultOperation` | حذف نرم تنوع |
| 4 | دریافت با شناسه | `/api/admin/product/product-variants/get-by-id` | GET | `{id}` | `ResultOperation<ProductVariant>` | دریافت اطلاعات تنوع |
| 5 | لیست DataTable | `/api/admin/product/product-variants/get-all` | POST | `DataTableRequest` | `DataTableResponse<ProductVariant>` | لیست تنوع‌ها با صفحه‌بندی |
| 6 | Select2 | `/api/admin/product/product-variants/select2` | GET | `{search}` | `List<Select2Item>` | لیست تنوع‌ها برای دراپ‌داون |

### 2.3 دسته‌بندی‌ها (Categories)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد دسته‌بندی | `/api/admin/product/categories/create` | POST | `CreateCategoryCommand` | `ResultOperation` | ایجاد دسته‌بندی (پدر/فرزندی) |
| 2 | ویرایش دسته‌بندی | `/api/admin/product/categories/update` | PUT | `UpdateCategoryCommand` | `ResultOperation` | ویرایش دسته‌بندی |
| 3 | حذف دسته‌بندی | `/api/admin/product/categories/delete` | DELETE | `{Id}` | `ResultOperation` | حذف دسته‌بندی |
| 4 | دریافت با شناسه | `/api/admin/product/categories/get-by-id` | GET | `{id}` | `ResultOperation<Category>` | دریافت اطلاعات دسته‌بندی |
| 5 | لیست DataTable | `/api/admin/product/categories/get-all` | POST | `DataTableRequest` | `DataTableResponse<Category>` | لیست دسته‌بندی‌ها |
| 6 | Select2 | `/api/admin/product/categories/select2` | GET | `{search}` | `List<Select2Item>` | لیست دسته‌بندی‌ها |

### 2.4 برندها (Brands)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد برند | `/api/admin/product/brands/create` | POST | `CreateBrandCommand` | `ResultOperation` | ایجاد برند جدید |
| 2 | ویرایش برند | `/api/admin/product/brands/update` | PUT | `UpdateBrandCommand` | `ResultOperation` | ویرایش برند |
| 3 | حذف برند | `/api/admin/product/brands/delete` | DELETE | `{Id}` | `ResultOperation` | حذف برند (SetNull محصولات) |
| 4 | دریافت با شناسه | `/api/admin/product/brands/get-by-id` | GET | `{id}` | `ResultOperation<Brand>` | دریافت اطلاعات برند |
| 5 | لیست DataTable | `/api/admin/product/brands/get-all` | POST | `DataTableRequest` | `DataTableResponse<Brand>` | لیست برندها |
| 6 | Select2 | `/api/admin/product/brands/select2` | GET | `{search}` | `List<Select2Item>` | لیست برندها |

---

## 3. ماژول Pricing (قیمت‌گذاری)

### 3.1 ارزها (Currencies)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد ارز | `/api/admin/pricing/currencies/create` | POST | `CreateCurrencyCommand` | `ResultOperation` | ایجاد واحد پول جدید |
| 2 | ویرایش ارز | `/api/admin/pricing/currencies/update` | PUT | `UpdateCurrencyCommand` | `ResultOperation` | ویرایش نرخ تبدیل و اطلاعات |
| 3 | دریافت با شناسه | `/api/admin/pricing/currencies/get-by-id` | GET | `{id}` | `ResultOperation<Currency>` | دریافت اطلاعات ارز |
| 4 | لیست DataTable | `/api/admin/pricing/currencies/get-all` | POST | `DataTableRequest` | `DataTableResponse<Currency>` | لیست ارزها |
| 5 | Select2 | `/api/admin/pricing/currencies/select2` | GET | `{search}` | `List<Select2Item>` | لیست ارزها |

### 3.2 قیمت‌ها (Prices)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد قیمت | `/api/admin/pricing/prices/create` | POST | `CreatePriceCommand` | `ResultOperation` | ثبت قیمت جدید (بستن قیمت قبلی خودکار) |
| 2 | بروزرسانی گروهی | `/api/admin/pricing/prices/bulk-update` | POST | `BulkPriceUpdateCommand` | `ResultOperation` | افزایش درصدی قیمت‌ها بر اساس دسته/محصول |
| 3 | قیمت‌های تنوع | `/api/admin/pricing/prices/get-by-variant-id` | GET | `{variantId}` | `ResultOperation<List<Price>>` | لیست قیمت‌های یک تنوع |
| 4 | تاریخچه قیمت | `/api/admin/pricing/prices/get-history` | GET | `{variantId, priceType?}` | `ResultOperation<List<Price>>` | تاریخچه تغییرات قیمت |

---

## 4. ماژول Inventory (انبار و موجودی)

### 4.1 موجودی (Stock)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ورود کالا | `/api/admin/inventory/stock/stock-in` | POST | `StockInCommand` | `ResultOperation` | ثبت ورودی موجودی |
| 2 | خروج کالا | `/api/admin/inventory/stock/stock-out` | POST | `StockOutCommand` | `ResultOperation` | ثبت خروجی (با بررسی موجودی) |
| 3 | تعدیل موجودی | `/api/admin/inventory/stock/adjust` | POST | `AdjustStockCommand` | `ResultOperation` | تعدیل دستی موجودی |
| 4 | موجودی اولیه | `/api/admin/inventory/stock/initial` | POST | `InitialStockCommand` | `ResultOperation` | ثبت موجودی اولیه (یکبار) |
| 5 | موجودی فعلی | `/api/admin/inventory/stock/get-current` | GET | `{variantId, warehouseId?}` | `ResultOperation<CurrentStockDto>` | محاسبه موجودی فعلی |
| 6 | تاریخچه موجودی | `/api/admin/inventory/stock/get-history` | GET | `{variantId, warehouseId?}` | `ResultOperation<List<StockLedger>>` | تاریخچه تغییرات |

### 4.2 انبارها (Warehouses)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد انبار | `/api/admin/inventory/warehouses/create` | POST | `CreateWarehouseCommand` | `ResultOperation` | ایجاد انبار جدید |
| 2 | ویرایش انبار | `/api/admin/inventory/warehouses/update` | PUT | `UpdateWarehouseCommand` | `ResultOperation` | ویرایش اطلاعات انبار |
| 3 | دریافت با شناسه | `/api/admin/inventory/warehouses/get-by-id` | GET | `{id}` | `ResultOperation<Warehouse>` | دریافت اطلاعات انبار |
| 4 | لیست DataTable | `/api/admin/inventory/warehouses/get-all` | POST | `DataTableRequest` | `DataTableResponse<Warehouse>` | لیست انبارها |
| 5 | Select2 | `/api/admin/inventory/warehouses/select2` | GET | `{search}` | `List<Select2Item>` | لیست انبارها |

---

## 5. ماژول Order (سفارش)

### 5.1 سبد خرید (Carts)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | افزودن به سبد | `/api/admin/order/carts/add-item` | POST | `AddToCartCommand` | `ResultOperation` | افزودن محصول به سبد خرید |
| 2 | حذف از سبد | `/api/admin/order/carts/remove-item` | POST | `RemoveFromCartCommand` | `ResultOperation` | حذف آیتم از سبد |
| 3 | تغییر تعداد | `/api/admin/order/carts/update-quantity` | POST | `UpdateCartItemQuantityCommand` | `ResultOperation` | تغییر تعداد آیتم سبد |
| 4 | اعمال تخفیف | `/api/admin/order/carts/apply-discount` | POST | `ApplyDiscountToCartCommand` | `ResultOperation` | اعمال کد تخفیف |
| 5 | سبد فعال | `/api/admin/order/carts/get-active` | GET | `{userId}` | `ResultOperation<Cart>` | دریافت سبد خرید فعال |

### 5.2 سفارشات (Orders)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد سفارش | `/api/admin/order/orders/create` | POST | `CreateOrderCommand` | `ResultOperation<long>` | ثبت سفارش از سبد خرید + رزرو موجودی |
| 2 | لغو سفارش | `/api/admin/order/orders/cancel` | POST | `CancelOrderCommand` | `ResultOperation` | لغو سفارش + آزادسازی رزرو |
| 3 | تغییر وضعیت | `/api/admin/order/orders/update-status` | POST | `UpdateOrderStatusCommand` | `ResultOperation` | تغییر وضعیت سفارش |
| 4 | دریافت با شناسه | `/api/admin/order/orders/get-by-id` | GET | `{id}` | `ResultOperation<Order>` | دریافت اطلاعات سفارش |
| 5 | لیست DataTable | `/api/admin/order/orders/get-all` | POST | `DataTableRequest` | `DataTableResponse<Order>` | لیست سفارشات |

---

## 6. ماژول Discount (تخفیف)

### 6.1 تخفیف‌ها (Discounts)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد تخفیف | `/api/admin/discount/discounts/create` | POST | `CreateDiscountCommand` | `ResultOperation` | ایجاد کد تخفیف |
| 2 | ویرایش تخفیف | `/api/admin/discount/discounts/update` | PUT | `UpdateDiscountCommand` | `ResultOperation` | ویرایش تخفیف |
| 3 | حذف تخفیف | `/api/admin/discount/discounts/delete` | DELETE | `{Id}` | `ResultOperation` | حذف تخفیف |
| 4 | دریافت با شناسه | `/api/admin/discount/discounts/get-by-id` | GET | `{id}` | `ResultOperation<Discount>` | دریافت اطلاعات تخفیف |
| 5 | لیست DataTable | `/api/admin/discount/discounts/get-all` | POST | `DataTableRequest` | `DataTableResponse<Discount>` | لیست تخفیف‌ها |
| 6 | اعتبارسنجی کد | `/api/admin/discount/discounts/validate-code` | GET | `{code}` | `ResultOperation<Discount>` | بررسی اعتبار کد تخفیف |

---

## 7. ماژول Payment (پرداخت)

### 7.1 پرداخت‌ها (Payments)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد پرداخت | `/api/admin/payment/payments/create` | POST | `CreatePaymentCommand` | `ResultOperation<Payment>` | ایجاد تراکنش پرداخت |
| 2 | تأیید پرداخت | `/api/admin/payment/payments/verify` | POST | `VerifyPaymentCommand` | `ResultOperation` | تأیید پرداخت از درگاه |
| 3 | بازپرداخت | `/api/admin/payment/payments/refund` | POST | `RefundPaymentCommand` | `ResultOperation` | بازپرداخت مبلغ |
| 4 | دریافت با شناسه | `/api/admin/payment/payments/get-by-id` | GET | `{id}` | `ResultOperation<Payment>` | دریافت اطلاعات پرداخت |
| 5 | پرداخت‌های سفارش | `/api/admin/payment/payments/get-by-order-id` | GET | `{orderId}` | `ResultOperation<List<Payment>>` | لیست پرداخت‌های سفارش |

### 7.2 درگاه‌ها (Gateways)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد درگاه | `/api/admin/payment/gateways/create` | POST | `CreatePaymentGatewayCommand` | `ResultOperation` | ایجاد درگاه پرداخت |
| 2 | ویرایش درگاه | `/api/admin/payment/gateways/update` | PUT | `UpdatePaymentGatewayCommand` | `ResultOperation` | ویرایش تنظیمات درگاه |
| 3 | لیست درگاه‌ها | `/api/admin/payment/gateways/get-all` | GET | — | `ResultOperation<List<PaymentGateway>>` | لیست تمام درگاه‌ها |
| 4 | درگاه‌های فعال | `/api/admin/payment/gateways/get-active` | GET | — | `ResultOperation<List<PaymentGateway>>` | لیست درگاه‌های فعال |

---

## 8. ماژول Shipping (ارسال و تحویل)

### 8.1 حاملان بار (Carriers)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد حامل | `/api/admin/shipping/carriers/create` | POST | `CreateCarrierCommand` | `ResultOperation` | ایجاد شرکت حمل‌ونقل |
| 2 | ویرایش حامل | `/api/admin/shipping/carriers/update` | PUT | `UpdateCarrierCommand` | `ResultOperation` | ویرایش اطلاعات حامل |
| 3 | لیست حاملان | `/api/admin/shipping/carriers/get-all` | GET | — | `ResultOperation<List<Carrier>>` | لیست حاملان بار |
| 4 | Select2 | `/api/admin/shipping/carriers/select2` | GET | `{search}` | `List<Select2Item>` | لیست حاملان |

### 8.2 روش‌های ارسال (DeliveryMethods)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد روش | `/api/admin/shipping/delivery-methods/create` | POST | `CreateDeliveryMethodCommand` | `ResultOperation` | ایجاد روش ارسال |
| 2 | ویرایش روش | `/api/admin/shipping/delivery-methods/update` | PUT | `UpdateDeliveryMethodCommand` | `ResultOperation` | ویرایش روش ارسال |
| 3 | لیست روش‌ها | `/api/admin/shipping/delivery-methods/get-all` | GET | — | `ResultOperation<List<DeliveryMethod>>` | لیست روش‌های ارسال |
| 4 | Select2 | `/api/admin/shipping/delivery-methods/select2` | GET | `{search}` | `List<Select2Item>` | لیست روش‌های ارسال |

### 8.3 تحویل سفارش (OrderDeliveries)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | تحویل سفارش | `/api/admin/shipping/order-deliveries/get-by-order-id` | GET | `{orderId}` | `ResultOperation<OrderDelivery>` | اطلاعات تحویل سفارش |
| 2 | تغییر وضعیت | `/api/admin/shipping/order-deliveries/update-status` | POST | `UpdateDeliveryStatusCommand` | `ResultOperation` | تغییر وضعیت تحویل |
| 3 | ثبت کد رهگیری | `/api/admin/shipping/order-deliveries/set-tracking-code` | POST | `SetTrackingCodeCommand` | `ResultOperation` | ثبت کد رهگیری پستی |

---

## 9. ماژول Accounting (حسابداری)

### 9.1 حساب‌ها (Accounts)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد حساب | `/api/admin/accounting/accounts/create` | POST | `CreateAccountCommand` | `ResultOperation` | ایجاد حساب حسابداری |
| 2 | ویرایش حساب | `/api/admin/accounting/accounts/update` | PUT | `UpdateAccountCommand` | `ResultOperation` | ویرایش حساب |
| 3 | دریافت با شناسه | `/api/admin/accounting/accounts/get-by-id` | GET | `{id}` | `ResultOperation<Account>` | دریافت اطلاعات حساب |
| 4 | لیست DataTable | `/api/admin/accounting/accounts/get-all` | POST | `DataTableRequest` | `DataTableResponse<Account>` | لیست حساب‌ها |
| 5 | Select2 | `/api/admin/accounting/accounts/select2` | GET | `{search}` | `List<Select2Item>` | لیست حساب‌ها |

### 9.2 تراکنش‌ها (Transactions)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ایجاد دسته‌ای | `/api/admin/accounting/transactions/create-batch` | POST | `CreateTransactionBatchCommand` | `ResultOperation` | ثبت تراکنش‌های حسابداری (دوطرفه) |
| 2 | تراکنش‌های حساب | `/api/admin/accounting/transactions/get-by-account-id` | GET | `{accountId}` | `ResultOperation<List<Transaction>>` | لیست تراکنش‌های حساب |
| 3 | تراکنش‌های دسته | `/api/admin/accounting/transactions/get-by-batch-id` | GET | `{batchId}` | `ResultOperation<List<Transaction>>` | لیست تراکنش‌های یک دسته |

---

## 10. ماژول Vendor (فروشنده)

### 10.1 فروشندگان (Vendors)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | ثبت‌نام فروشنده | `/api/admin/vendor/vendors/create` | POST | `CreateVendorCommand` | `ResultOperation` | ثبت‌نام فروشنده جدید (Status=Pending) |
| 2 | ویرایش فروشنده | `/api/admin/vendor/vendors/update` | PUT | `UpdateVendorCommand` | `ResultOperation` | ویرایش اطلاعات فروشنده |
| 3 | تأیید فروشنده | `/api/admin/vendor/vendors/approve` | POST | `ApproveVendorCommand` | `ResultOperation` | تأیید فروشنده (Pending→Active) |
| 4 | تعلیق فروشنده | `/api/admin/vendor/vendors/suspend` | POST | `SuspendVendorCommand` | `ResultOperation` | تعلیق فروشنده (Active→Suspended) |
| 5 | دریافت با شناسه | `/api/admin/vendor/vendors/get-by-id` | GET | `{id}` | `ResultOperation<Vendor>` | دریافت اطلاعات فروشنده |
| 6 | لیست DataTable | `/api/admin/vendor/vendors/get-all` | POST | `DataTableRequest` | `DataTableResponse<Vendor>` | لیست فروشندگان |
| 7 | Select2 | `/api/admin/vendor/vendors/select2` | GET | `{search}` | `List<Select2Item>` | لیست فروشندگان |

---

## 11. ماژول Settings (تنظیمات)

### 11.1 تنظیمات (Settings)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | دریافت با کلید | `/api/admin/settings/settings/get-by-key` | GET | `{key}` | `ResultOperation<Setting>` | دریافت تنظیم با کلید |
| 2 | تنظیمات ماژول | `/api/admin/settings/settings/get-by-module` | GET | `{module}` | `ResultOperation<List<Setting>>` | تنظیمات یک ماژول |
| 3 | تمام تنظیمات | `/api/admin/settings/settings/get-all` | GET | — | `ResultOperation<List<Setting>>` | لیست تمام تنظیمات |
| 4 | ویرایش تنظیم | `/api/admin/settings/settings/update` | PUT | `UpdateSettingCommand` | `ResultOperation` | ویرایش یک تنظیم |
| 5 | بروزرسانی گروهی | `/api/admin/settings/settings/bulk-update` | POST | `BulkUpdateSettingsCommand` | `ResultOperation` | ویرایش چندین تنظیم |

### 11.2 مقادیر شمارش (EnumValues)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | مقادیر نوع | `/api/admin/settings/enum-values/get-by-type` | GET | `{enumType}` | `ResultOperation<List<EnumValue>>` | مقادیر یک شمارش |
| 2 | ایجاد مقدار | `/api/admin/settings/enum-values/create` | POST | `CreateEnumValueCommand` | `ResultOperation` | ایجاد مقدار شمارش |
| 3 | ویرایش مقدار | `/api/admin/settings/enum-values/update` | PUT | `UpdateEnumValueCommand` | `ResultOperation` | ویرایش مقدار شمارش |

---

## 12. ماژول Customization (شخصی‌سازی)

### 12.1 پوسته‌ها (Themes)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | پوسته فعال | `/api/admin/customization/themes/get-active` | GET | — | `ResultOperation<SiteTheme>` | دریافت پوسته فعال |
| 2 | لیست پوسته‌ها | `/api/admin/customization/themes/get-all` | GET | — | `ResultOperation<List<SiteTheme>>` | لیست پوسته‌ها |
| 3 | ایجاد پوسته | `/api/admin/customization/themes/create` | POST | `CreateSiteThemeCommand` | `ResultOperation` | ایجاد پوسته جدید |
| 4 | ویرایش پوسته | `/api/admin/customization/themes/update` | PUT | `UpdateSiteThemeCommand` | `ResultOperation` | ویرایش پوسته |
| 5 | فعال‌سازی | `/api/admin/customization/themes/activate` | POST | `ActivateSiteThemeCommand` | `ResultOperation` | فعال‌سازی پوسته (غیرفعال کردن بقیه) |

### 12.2 اسلایدرها (Sliders)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | لیست اسلایدرها | `/api/admin/customization/sliders/get-all` | GET | — | `ResultOperation<List<Slider>>` | لیست اسلایدرها |
| 2 | اسلایدرهای فعال | `/api/admin/customization/sliders/get-active` | GET | — | `ResultOperation<List<Slider>>` | اسلایدرهای فعال |
| 3 | ایجاد اسلایدر | `/api/admin/customization/sliders/create` | POST | `CreateSliderCommand` | `ResultOperation` | ایجاد اسلایدر |
| 4 | ویرایش اسلایدر | `/api/admin/customization/sliders/update` | PUT | `UpdateSliderCommand` | `ResultOperation` | ویرایش اسلایدر |
| 5 | حذف اسلایدر | `/api/admin/customization/sliders/delete` | DELETE | `{Id}` | `ResultOperation` | حذف اسلایدر |

### 12.3 ویجت‌ها (Widgets)

| # | سرویس | آدرس | متد | ورودی | خروجی | توضیح |
|---|-------|------|------|--------|-------|-------|
| 1 | لیست ویجت‌ها | `/api/admin/customization/widgets/get-all` | GET | — | `ResultOperation<List<Widget>>` | لیست ویجت‌ها |
| 2 | ویجت‌های موقعیت | `/api/admin/customization/widgets/get-by-position` | GET | `{position}` | `ResultOperation<List<Widget>>` | ویجت‌های یک موقعیت |
| 3 | ایجاد ویجت | `/api/admin/customization/widgets/create` | POST | `CreateWidgetCommand` | `ResultOperation` | ایجاد ویجت |
| 4 | ویرایش ویجت | `/api/admin/customization/widgets/update` | PUT | `UpdateWidgetCommand` | `ResultOperation` | ویرایش ویجت |
| 5 | حذف ویجت | `/api/admin/customization/widgets/delete` | DELETE | `{Id}` | `ResultOperation` | حذف ویجت |
