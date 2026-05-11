
# SmartDataTable – Quick Start Guide

ماژول SmartDataTable یک لایه‌ی ساده روی **jQuery DataTables** است که امکان تعریف جدول‌ها را فقط با **HTML attributes** فراهم می‌کند و نیاز به JavaScript اضافی را تا حد زیادی حذف می‌کند.

ویژگی‌ها:

- Auto initialization با attribute
- تعریف ستون‌ها از داخل `<thead>`
- پشتیبانی از template در ستون‌ها
- سیستم event برای کنترل جدول
- فیلترهای خارجی
- سیستم دکمه‌های extensible
- قابلیت افزودن renderer برای نوع ستون

---

# نصب

ابتدا کتابخانه‌های زیر باید در پروژه وجود داشته باشند:

- jQuery
- DataTables
- DataTables Buttons extension
- فایل `SmartDataTable.js`

---

# شروع سریع (Quick Start)

کافیست به جدول خود attribute زیر را اضافه کنید:

```html
<table id="usersTable"
       data-smart-dt
       data-dt-url="/api/users">
```

و ستون‌ها را در `thead` تعریف کنید:

```html
<table id="usersTable"
       data-smart-dt
       data-dt-url="/api/users">

<thead>
<tr>
<th data-dt-data="id">شناسه</th>
<th data-dt-data="userName">نام کاربری</th>
<th data-dt-data="email">ایمیل</th>
</tr>
</thead>

</table>
```

با load شدن صفحه جدول **به صورت خودکار ساخته می‌شود**.

---

# کنترل جدول با JavaScript

برای کنترل جدول‌ها از سیستم event استفاده می‌شود.

### Reload جدول

```javascript
SmartDtFireEvent("usersTable","reload");
```

---

### Reload با reset صفحه

```javascript
SmartDtFireEvent("usersTable","reload-reset");
```

---

### جستجو

```javascript
SmartDtFireEvent("usersTable","search","ali");
```

---

### تغییر صفحه

```javascript
SmartDtFireEvent("usersTable","page",2);
```

---

# Attribute های جدول

| Attribute | توضیح |
|---|---|
| data-smart-dt | فعال کردن SmartDataTable |
| data-dt-url | آدرس ajax |
| data-dt-method | نوع درخواست (GET / POST) |
| data-dt-page-length | تعداد رکورد در صفحه |
| data-dt-searching | فعال / غیرفعال کردن search |
| data-dt-order | ترتیب پیشفرض مثال `0:desc` |
| data-dt-buttons | لیست دکمه‌ها |
| data-dt-filters | selector فیلترهای خارجی |
| data-dt-reload-on | selector برای reload |

---

# Attribute های ستون (th)

| Attribute | توضیح |
|---|---|
| data-dt-data | نام فیلد از API |
| data-dt-name | نام ستون |
| data-dt-type | نوع renderer |
| data-dt-width | عرض ستون |
| data-dt-class | کلاس CSS |
| data-dt-orderable | قابل مرتب سازی |
| data-dt-searchable | قابل جستجو |

---

# انواع Column Type

### jalali

نمایش تاریخ شمسی

```html
<th data-dt-data="createdAt"
    data-dt-type="jalali">
تاریخ
</th>
```

---

### gregorian

نمایش تاریخ میلادی

```html
<th data-dt-data="createdAt"
    data-dt-type="gregorian">
Date
</th>
```

---

### yesno

نمایش آیکون true / false

```html
<th data-dt-data="isActive"
    data-dt-type="yesno">
وضعیت
</th>
```

---

### badge

نمایش مقدار داخل badge

```html
<th data-dt-data="role"
    data-dt-type="badge">
Role
</th>
```

---

# Template در ستون‌ها

برای ایجاد HTML سفارشی در ستون‌ها می‌توان از `<template>` استفاده کرد.

مثال:

```html
<th data-dt-data="actions">

<template>
<button data-id="{{row.id}}">
Edit
</button>
</template>

Actions

</th>
```

متغیرهای قابل استفاده:

| متغیر | توضیح |
|---|---|
| {{data}} | مقدار ستون |
| {{row}} | کل ردیف |
| {{row.id}} | فیلد خاص |

---

# فیلترهای خارجی

می‌توان input های خارج از جدول را به عنوان فیلتر استفاده کرد.

```html
<input name="email"
       data-dt-target="#usersTable"
       data-dt-auto-search>
```

ویژگی‌ها:

| Attribute | توضیح |
|---|---|
| data-dt-target | جدول هدف |
| data-dt-auto-search | جستجوی خودکار |
| name | نام پارامتر ارسال به سرور |

---

# دکمه‌ها (Buttons)

دکمه‌ها با attribute زیر فعال می‌شوند:

```html
data-dt-buttons="excelAll,csvCurrent"
```

دکمه‌های پیشفرض:

| Button | توضیح |
|---|---|
| excelAll | خروجی Excel همه رکوردها |
| excelCurrent | خروجی Excel صفحه جاری |
| csvCurrent | خروجی CSV |

---

# اضافه کردن Column Type جدید

```javascript
SmartDataTable.registerType("currency", function(data){
return data + " تومان";
});
```

استفاده:

```html
<th data-dt-data="price"
    data-dt-type="currency">
Price
</th>
```

---

# اضافه کردن Button جدید

```javascript
SmartDataTable.registerButton("myButton", function(dt){

return {
text:"My Button",
className:"btn btn-primary",
action:function(){
alert("clicked");
}
};

});
```

استفاده:

```html
data-dt-buttons="myButton"
```

---

# Init دستی (اختیاری)

در صورت نیاز می‌توان جدول را دستی ایجاد کرد:

```javascript
initSmartDataTable("#usersTable");
```

---

# مثال کامل

```html
<table id="usersTable"
       data-smart-dt
       data-dt-url="/api/users"
       data-dt-order="0:desc"
       data-dt-page-length="10"
       data-dt-buttons="excelAll,csvCurrent">

<thead>

<tr>

<th data-dt-data="id">شناسه</th>

<th data-dt-data="userName">
نام کاربری
</th>

<th data-dt-data="createdAt"
    data-dt-type="jalali">
تاریخ ثبت
</th>

<th data-dt-data="isActive"
    data-dt-type="yesno">
وضعیت
</th>

<th data-dt-data="actions"
    data-dt-orderable="false">

<template>

<button class="btn btn-sm btn-danger"
        data-id="{{row.id}}">
حذف
</button>

</template>

عملیات

</th>

</tr>

</thead>

</table>
```

---

# ساختار API مورد انتظار

SmartDataTable از **DataTables server-side format** استفاده می‌کند.

نمونه response:

```json
{
 "draw":1,
 "recordsTotal":100,
 "recordsFiltered":100,
 "data":[
   {
     "id":1,
     "userName":"admin",
     "email":"admin@test.com"
   }
 ]
}
```

