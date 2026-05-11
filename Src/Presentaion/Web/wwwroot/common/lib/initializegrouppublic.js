/**
 * ایجاد یک Select2 با داده‌های دریافتی از API
 * @param {string} elementId - آیدی المان select (مثلاً 'txtDomainKind')
 * @param {number|string} groupId - شناسه گروه/نوع/دسته‌بندی که به API ارسال می‌شود (اختیاری)
 * @param {string} apiUrl - آدرس پایه API (بدون query string)
 * @param {Object} [options={}] - تنظیمات اختیاری
 * @param {string} [options.placeholder='یک گزینه انتخاب کنید']
 * @param {boolean} [options.allowClear=true]
 * @param {number} [options.minimumInputLength=0]
 * @param {string} [options.defaultValue='0']
 * @param {string} [options.defaultText='همه'] - متن گزینه پیش‌فرض (همه)
 * @param {string} [options.queryParamName='groupId'] - نام پارامتر کوئری که ارسال می‌شود
 */
async function initializegrouppublic(elementId, groupId, apiUrl, options = {}) {
  // تنظیمات پیش‌فرض
  const {
    placeholder = 'یک گزینه انتخاب کنید',
    allowClear = true,
    minimumInputLength = 0,
    defaultValue = '0',
    defaultText = 'همه',
    queryParamName = 'groupId'   // ← می‌تونی نام پارامتر رو تغییر بدی (مثلاً categoryId، typeId و ...)
  } = options;

  // اگر groupId وجود داشت، به URL اضافه کن
  let finalUrl = apiUrl;
  if (groupId !== undefined && groupId !== null && groupId !== '') {
    finalUrl += `?${queryParamName}=${encodeURIComponent(groupId)}`;
  }

  try {
    const response = await fetch(finalUrl);
    if (!response.ok) {
      throw new Error(`خطا در دریافت داده - وضعیت: ${response.status}`);
    }

    const data = await response.json();
    const items = data.results || data || [];  // پشتیبانی از فرمت‌های مختلف API

    const selectElement = document.getElementById(elementId);
    if (!selectElement) {
      console.error(`المان با آیدی ${elementId} یافت نشد`);
      return;
    }

    // پاک کردن گزینه‌های قبلی
    selectElement.innerHTML = '';

    // ۱. گزینه خالی برای allowClear
    if (allowClear) {
      const emptyOption = document.createElement('option');
      emptyOption.value = '';
      emptyOption.textContent = '';
      selectElement.appendChild(emptyOption);
    }

    // ۲. گزینه پیش‌فرض "همه"
    const allOption = document.createElement('option');
    allOption.value = defaultValue;
    allOption.textContent = defaultText;
    selectElement.appendChild(allOption);

    // ۳. اضافه کردن گزینه‌های دریافتی از API
    items.forEach(item => {
      const option = document.createElement('option');
      option.value = item.id ?? item.value ?? '';       // پشتیبانی از id یا value
      option.textContent = item.text ?? item.name ?? item.title ?? 'بدون عنوان';
      selectElement.appendChild(option);
    });

    // ۴. مقداردهی اولیه و فعال‌سازی Select2
    const $select = $(`#${elementId}`);
    $select.select2({
      placeholder: placeholder,
      allowClear: allowClear,
      minimumInputLength: minimumInputLength,
      // اگر می‌خوای جستجوی سمت سرور داشته باشی، می‌تونی ajax هم اضافه کنی
    });

    // اختیاری: اگر می‌خوای بعد از لود یک مقدار پیش‌فرض انتخاب بشه
    // $select.val(defaultValue).trigger('change');

    console.log(`Select2 با موفقیت برای ${elementId} ساخته شد`);

  } catch (error) {
    console.error(`خطا در ساخت Select2 برای ${elementId}:`, error);
    // می‌تونی اینجا یک پیام به کاربر نشون بدی یا گزینه‌ای پیش‌فرض بذاری
  }
}
