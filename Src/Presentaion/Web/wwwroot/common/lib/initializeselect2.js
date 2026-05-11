'use strict';

/**
 * تابع عمومی برای راه‌اندازی هر Select2 با AJAX و تنظیم مقدار اولیه
 * 
 * @param {Object} options - تنظیمات ورودی
 * @param {jQuery} options.$ELEMENT          - المان select (مثل $('#CourseId'))
 * @param {string} options.apiUrl            - URL پایه API
 * @param {string|number} [options.domainId] - (اختیاری) domainId برای URL (مثل دوره)
 * @param {string} [options.placeholder]     - متن placeholder
 * @param {string} [options.currentId]       - مقدار اولیه (اگر نذاری از $element.val() می‌گیره)
 * @param {Object} [options.extraParams]     - پارامترهای اضافی برای درخواست مقدار اولیه (مثل {id: 123})
 * @param {string} [options.label]           - نام برای لاگ‌ها (مثل "دوره" یا "کاربر")
 * @param {jQuery} [options.dropdownParent]  - parent برای dropdown (پیش‌فرض: #PaymentDiscountModal)
 * @param {function} [options.onSelect]      - کال‌بک سفارشی برای رویداد select
 * @param {function} [options.onClear]       - کال‌بک سفارشی برای رویداد clear
 */
'use strict';

/**
 * تابع عمومی برای راه‌اندازی Select2 با AJAX و تنظیم مقدار اولیه
 */
function initializeSelect2(options) {
  var $select = options.$element;
  var apiUrl = options.apiUrl;
  var domainId = options.domainId || null;
  var permissionLevel = options.permissionLevel ?? null;
  var placeholder = options.placeholder || "جستجو و انتخاب...";
  var currentId = options.currentId !== undefined ? options.currentId : $select.val();
  var extraParams = options.extraParams || {};
  var label = options.label || "آیتم";
  var dropdownParent = options.dropdownParent || $select.parent();

  if (!$select.length || !apiUrl) {
    console.error("المان یا آدرس API معتبر نیست.");
    return;
  }

  // اگر قبلاً ساخته شده، destroy کن (برای جلوگیری از تکرار)
  if ($select.hasClass('select2-hidden-accessible')) {
    $select.select2('destroy');
  }

  // راه‌اندازی Select2
  $select.select2({
    ajax: {
      url: function (params) {
        var term = params.term || "";
        var page = params.page || 1;
        var length = 10;

        var url = `${apiUrl}?searchQuery=${encodeURIComponent(term)}&length=${length}&page=${page}`;
        if (domainId !== null) {
          url += `&domainId=${domainId}`;
        }
        if (permissionLevel === 0 && permissionLevel !== null) {
          url += `&permissionLevel=${permissionLevel}`;
        }
        return url;
      },
      dataType: 'json',
      delay: 300,
      cache: true,
      processResults: function (data) {
        return {
          results: data.results || [],
          pagination: { more: !!data.pagination?.more }
        };
      }
    },
    placeholder: placeholder,
    allowClear: true,
    minimumInputLength: 0,
    language: {
      noResults: () => "نتیجه‌ای یافت نشد",
      searching: () => "در حال جستجو..."
    },
    dropdownParent: dropdownParent  // دراپ‌داون داخل modal باز می‌شه
  });

  // تنظیم مقدار اولیه در حالت ویرایش
  if (currentId && currentId !== "0" && currentId !== "") {
    var initialUrl = `${apiUrl}?searchQuery=${currentId}&length=10&page=1`;
    if (domainId !== null) {
      initialUrl += `&domainId=${domainId}`;
    }
    if (permissionLevel === 0 && permissionLevel !== null) {
      initialUrl += `&permissionLevel=${permissionLevel}`;
    }
    Object.keys(extraParams).forEach(function (key) {
      initialUrl += `&${key}=${encodeURIComponent(extraParams[key])}`;
    });

    $.ajax({
      url: initialUrl,
      dataType: 'json',
      cache: true,
      success: function (data) {
        if (data?.results && data.results.length > 0) {

          var item = data.results.find(r => String(r.id) === String(currentId));

          // اگر آیتم وجود نداشت هیچ کاری نکن
          if (!item) {
            console.warn(`${label} با id ${currentId} در لیست وجود ندارد.`);
            return;
          }

          var option = new Option(item.text, item.id, true, true);
          $select.append(option).trigger('change');

          $select.trigger({
            type: 'select2:select',
            params: { data: item }
          });

        } else {
          console.warn(`${label} با id ${currentId} یافت نشد.`);
        }
      }
      
    });
  }

  // رویدادها
  $select.on('select2:select', function (e) {
    var data = e.params.data;
    console.log(`${label} انتخاب شد:`, data.id, data.text);
    if (typeof options.onSelect === 'function') options.onSelect(e);
  });

  $select.on('select2:clear', function () {
    console.log(`${label} پاک شد`);
    if (typeof options.onClear === 'function') options.onClear();
  });
}

