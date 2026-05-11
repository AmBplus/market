# 🔔 AmbModal — Notification Modal

یک ماژول مودال اعلان سبک و زیبا بدون هیچ dependency خارجی.

---

## 📦 نصب

```html
<script src="/wwwroot/js/amb-modal-module/amb-modal.js"></script>
```

---

## 🚀 استفاده سریع

```javascript
// خطا
AmbModal.error('نام کاربری یا رمز عبور اشتباه است');

// موفقیت
AmbModal.success('عملیات با موفقیت انجام شد');

// هشدار
AmbModal.warning('این عملیات قابل بازگشت نیست');

// اطلاعات
AmbModal.info('لطفاً اطلاعات خود را تکمیل کنید');

// چند پیام
AmbModal.error(['فیلد نام الزامی است', 'ایمیل معتبر نیست']);
```

---

## ⚙️ تنظیمات کامل

```javascript
AmbModal.show({
    type: 'error',              // 'error' | 'success' | 'warning' | 'info'
    title: 'عنوان دلخواه',      // اختیاری — پیش‌فرض: نام type
    messages: ['پیام ۱', 'پیام ۲'],  // string یا string[]
    confirmText: 'باشه',        // اختیاری — پیش‌فرض: 'باشه'
    cancelText: 'انصراف',       // اختیاری — null = نمایش داده نمی‌شود
    onConfirm: () => { ... },   // callback تأیید
    onCancel:  () => { ... },   // callback لغو
});
```

---

## 📋 انواع مودال

| type | رنگ | کاربرد |
|------|-----|--------|
| `error` | قرمز | خطاهای اعتبارسنجی، عملیات ناموفق |
| `success` | سبز | عملیات موفق |
| `warning` | زرد | هشدار قبل از عملیات مهم |
| `info` | آبی | اطلاع‌رسانی |

---

## 🎨 ویژگی‌ها

- ✅ پشتیبانی از CSS Variables (تم روشن/تاریک)
- ✅ انیمیشن ورود/خروج
- ✅ بستن با کلیک خارج از باکس
- ✅ بستن با کلید Escape
- ✅ نمایش چند پیام به صورت لیست
- ✅ RTL کامل
- ✅ بدون dependency

---

## 📁 ساختار فایل‌ها

```
wwwroot/js/amb-modal-module/
├── amb-modal.js   ← ماژول اصلی
└── README.md      ← این فایل
```

---

*ساخته شده با ❤️ برای سامانه دانا*
