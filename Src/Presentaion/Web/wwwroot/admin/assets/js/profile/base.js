$(function () {

  //// Date Picker Settings
  //var currentBirthDay = $("#firstBirthDate").val();
  //if (!currentBirthDay || currentBirthDay === "") {
  //  currentBirthDay = new Date().toISOString().split('T')[0];
  //}
  //new mds.MdsPersianDateTimePicker(document.querySelector('#date-text'), {
  //  targetTextSelector: '#date-text',
  //  targetDateSelector: '#Profile_BirthDate',
  //  isGregorian: isGregorian,
  //  calendarViewOnChange: function (date) {
  //  },
  //  selectedDate: new Date(currentBirthDay),
  //  selectedDateToShow: new Date(currentBirthDay)
  //});

  // Select the input element
  // const dateInput = document.getElementById("date-text");

  //// Function to convert Gregorian date to Persian date
  //function gregorianToPersian(gregorianDate) {

  //  return new Date(gregorianDate).toLocaleDateString('fa-IR');
  //}

  // Check if the input has a value
  //if (dateInput.value) {
  //  const persianDate = gregorianToPersian(dateInput.value);
  //  dateInput.value = persianDate; // Set the Persian date back to the input
  //}

  //jalaliDatepicker.startWatch({
  //  minDate: "attr",
  //  maxDate: "attr",
  //  minTime: "attr",
  //  maxTime: "attr",
  //  time: false,
  //  date: true,
  //  hasSecond: false,
  //  hideAfterChange: true,
  //  autoHide: true,
  //  showTodayBtn: true,
  //  showEmptyBtn: false,
  //  topSpace: 10,
  //  bottomSpace: 30,
  //  overflowSpace: -10,
  //  showCloseBtn: true,
  //  dayRendering(opt, input) {
  //    return {
  //      isHollyDay: opt.day == 1,
  //    };
  //  },
  //});

  //$(document).on("jdp:change", "#date-text", function (e) {
  //  console.log('jdp:change event triggered');

  //  // دسترسی به المان مورد نظر با jQuery
  //  var miladiInput = $("#Profile_BirthDate");
  //  console.log('miladi input current :', miladiInput.val())
  //  // بررسی مقدار ورودی
  //  console.log('this.value:', $(this).val());
  //  if (!$(this).val()) {
  //    miladiInput.val(""); // خالی کردن مقدار
  //    return;
  //  }

  //  // تبدیل تاریخ شمسی به میلادی
  //  var date = $(this).val().split("/");
  //  var gregorianDate = jalali_to_gregorian(date[0], date[1], date[2]);
  //  console.log('gregorianDate:', gregorianDate);

  //  // فرمت تاریخ میلادی به صورت YYYY-MM-DD
  //  var formattedGregorianDate = formatGregorianDate(gregorianDate);
  //  console.log('formattedGregorianDate:', formattedGregorianDate);

  //  // تنظیم مقدار جدید با jQuery
  //  miladiInput.val(formattedGregorianDate);
  //  setTimeout(function () {
  //    miladiInput.val(formattedGregorianDate);
  //    miladiInput.trigger('input');
  //    miladiInput.trigger('change');
  //    // بررسی مقدار نهایی
  //    console.log('Updated value:', miladiInput.val());
  //  }, 1); // تأخیر 10 میلی‌ثانیه
  //  // ارسال رویداد برای اعمال تغییرات


  //});
  //function jalali_to_gregorian(jy, jm, jd) {
  //  jy = Number(jy);
  //  jm = Number(jm);
  //  jd = Number(jd);
  //  var gy = (jy <= 979) ? 621 : 1600;
  //  jy -= (jy <= 979) ? 0 : 979;
  //  var days = (365 * jy) + ((parseInt(jy / 33)) * 8) + (parseInt(((jy % 33) + 3) / 4))
  //    + 78 + jd + ((jm < 7) ? (jm - 1) * 31 : ((jm - 7) * 30) + 186);
  //  gy += 400 * (parseInt(days / 146097));
  //  days %= 146097;
  //  if (days > 36524) {
  //    gy += 100 * (parseInt(--days / 36524));
  //    days %= 36524;
  //    if (days >= 365) days++;
  //  }
  //  gy += 4 * (parseInt((days) / 1461));
  //  days %= 1461;
  //  gy += parseInt((days - 1) / 365);
  //  if (days > 365) days = (days - 1) % 365;
  //  var gd = days + 1;
  //  var sal_a = [0, 31, ((gy % 4 == 0 && gy % 100 != 0) || (gy % 400 == 0)) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
  //  var gm
  //  for (gm = 0; gm < 13; gm++) {
  //    var v = sal_a[gm];
  //    if (gd <= v) break;
  //    gd -= v;
  //  }
  //  return [gy, gm, gd];
  //}

  var getCitiesByConturyCodeApi = $(".getCitiesByContryCodeApiLink").val();

  var selectElement = document.getElementById('Profile_Province');

  // Find the option with the class 'select'
  var selectProvince = $('#Profile_Province');
  var selectedOption = selectProvince.find('option.select');
  // Set the 'selected' attribute to trueFclear
  selectedOption.prop('selected', true);

  $('#Profile_Province').select2({
    placeholder: "انتخاب استان",
    language: {
      noResults: function () {
        return "نتیجه ای یافت نشد";  // Change this to your custom message
      }
    }
  })
  var select = $('#Profile_City').select2({
    placeholder: "انتخاب شهر",
    language: {
      noResults: function () {
        return "نتیجه ای یافت نشد";  // Change this to your custom message
      }
    }
  });

  // تابع برای فرمت‌دهی تاریخ میلادی به صورت YYYY-MM-DD
  function formatGregorianDate(gregorianDate) {
    var year = gregorianDate[0];
    var month = String(gregorianDate[1]).padStart(2, '0'); // ماه را دو رقمی می‌کنیم
    var day = String(gregorianDate[2]).padStart(2, '0'); // روز را دو رقمی می‌کنیم
    return `${year}/${month}/${day}`;
  }
  $('#Profile_Province').on('select2:select', function (e) {
    // Get selected option
    var selectedOption = e.params.data;
    // Get display text and value of selected option
    var value = selectedOption.id;
    // Perform the AJAX request
    $.ajax({
      url: getCitiesByConturyCodeApi + '?countryCode=' + value,
      type: 'GET',
      success: function (data) {
        // Process the results
        var results = data.data;
        // Initialize the Select2 dropdown
        // Clear any existing options
        select.empty();
        // Use a forEach loop to add the new options
        var defaultOption = new Option("انتخاب شهر", null, true, true);
        defaultOption.disabled = true;
        select.append(defaultOption)
        results.forEach(function (item) {
          var option = new Option(item.text, item.id, false, false);
          select.append(option);
        });
        // Trigger the change event to update the UI
        select.val(null).trigger('change');
      },
      error: function (jqXHR, textStatus, errorThrown) {
        // Handle any errors
        console.error('Error fetching data: ', textStatus, errorThrown);
      }
    });
  });

  // Upload Avatar
  var inputFileAvatar = document.getElementById('uploadavatar');
  document.getElementById('btn-upload-avatart').addEventListener('click', () => {
    inputFileAvatar.click();
  })
  inputFileAvatar.addEventListener('change', function (event) {
    // Get the file input element
    const input = event.target;
    // Get the selected file(s)
    const files = input.files;
    if (files.length > 0) {
      // Get the first file
      const file = files[0];
      // Validate file size (500 KB = 512000 bytes)
      if (file.size > 512000) {
        Swal.fire({
          title: "خطا",
          text: "فایل از 500 کلیو بایت بیشتر است",
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: "بله ، تغییر بده",
          customClass: {
            confirmButton: 'btn btn-primary me-3 waves-effect waves-light',
          },
          buttonsStyling: false
        })
        return;
      }
      //// You can perform any action here with the file
      //console.log('Selected file:', file);
      // Example: Display the file name
/*      alert('Selected file: ' + file.name);*/
      Swal.fire({
        title: "آیا مطمئنید؟",
        text: "در صورت تغییر پروفایل امکان برگرداندن نیست",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: "بله ، تغییر بده",
        cancelButtonText: "نه ، منصرف شدم",
        customClass: {
          confirmButton: 'btn btn-primary me-3 waves-effect waves-light',
          cancelButton: 'btn btn-label-secondary waves-effect waves-light'
        },
        buttonsStyling: false
      })
        .then(function (result) {
          if (result.value) {
            const formData = new FormData();
            formData.append('formFile', file);
            fetch('/api/personInfo/profile/UpdatePicProfile', {
              method: 'POST',
              body: formData
            })
              .then(response => response.json())
              .then(data => {
                if (data.isSuccess) {
                  const uploadedAvatar = document.getElementById('uploadedAvatar');
                  const fileURL = URL.createObjectURL(file);
                  uploadedAvatar.src = fileURL;
                  Swal.fire({
                    text: "عملیات با موفقیت انجام شد",
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonText: "باشه",
                    timer: 2500,
                    customClass: {
                      confirmButton: 'btn btn-primary me-3 waves-effect waves-light',
                    },
                    buttonsStyling: false
                  })
                } else {
                  alert('File upload failed');
                }
              })
              .catch(error => {
                console.error('Error uploading file:', error);
                alert('An error occurred while uploading the file');
              });
          }
     
        });
    } else {
      alert('No file selected');
    }
  });
})
