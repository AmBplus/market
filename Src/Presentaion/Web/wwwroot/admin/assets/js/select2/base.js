
'use strict';
$(function () {
  $('.select2').select2({
    allowClear: $(this).data('allow-clear'),
    language: {
      noResults: function () {
        return "نتیجه‌ای یافت نشد"; // Custom message in Persian
      }
    }
  });
});


