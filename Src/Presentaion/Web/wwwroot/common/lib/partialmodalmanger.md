# Razor Modal Partial Loader

سیستم عمومی برای بارگذاری Razor Partial View داخل Bootstrap Modal با پشتیبانی کامل از GET و POST.

---

# ویژگی‌ها

- بارگذاری partial داخل modal
- اجرای script داخل partial
- اعمال style داخل partial
- حذف خودکار script/style هنگام بسته شدن modal
- ارسال فرم با POST
- آپلود فایل با progress
- امکان cancel upload
- پشتیبانی از DataTable refresh
- جلوگیری از multiple binding
- پشتیبانی از چند modal همزمان
- کاملاً backward compatible

---

# Quick Start

## 1 افزودن Modal
```html
<div class="modal fade" id="userModal">
  <div class="modal-dialog">
<div class="modal-content">
<div class="modal-body"></div>
</div>
  </div>
</div>
