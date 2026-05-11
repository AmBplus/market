/**
 *  Page auth two steps
 */

'use strict';

document.addEventListener('DOMContentLoaded', function (e) {
  (function () {
    var minutes = 0; // محاسبه دقیقه
    var seconds = 0; // محاسبه ثانیه

    // تابع برای تنظیم کوکی
    function setConfrmMobileCookie(name, value, days) {
      var expires = "";
      if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
      }
      document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    // تابع برای خواندن کوکی
    function getConfirmMobileCookie(name) {
      var nameEQ = name + "=";
      var ca = document.cookie.split(';');
      for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
      }
      return null;
    }

    // تابع ترکیبی برای get یا set کوکی
    function getOrSetConfrmMobileCookie(name, value, days) {
      var cookieData = getConfirmMobileCookie(name);
      if (cookieData) {
        // اگر کوکی وجود داشت، مقدار آن را برگردان
        return cookieData;
      } else {
        // اگر کوکی وجود نداشت، آن را تنظیم کن
        setConfrmMobileCookie(name, value, days);
        return null; // یا می‌توانید مقدار تنظیم شده را برگردانید
      }
    }

    // خواندن مقدار فیلد SesionId
    var sesionId = document.getElementById('SesionId').value;

    // دریافت تاریخ فعلی
    var currentDate = new Date();

    // ترکیب تاریخ فعلی در یک شیء JSON
    var cookieValue = JSON.stringify({
      timestamp: currentDate.getTime() // ذخیره زمان به صورت timestamp
    });

    // نام کوکی منحصر به فرد
    var cookieName = 'SessionData_' + sesionId;

    // get یا set کوکی
    var existingCookie = getOrSetConfrmMobileCookie(cookieName, cookieValue, 1);

    // بررسی زمان سپری شده و محاسبه زمان باقی‌مانده
    function checkCookieExpiry() {
      var cookieData = existingCookie || getConfirmMobileCookie(cookieName);
      if (cookieData) {
        var data = JSON.parse(cookieData);
        var storedTime = data.timestamp; // زمان ذخیره‌سازی
        var currentTime = new Date().getTime(); // زمان فعلی
        var elapsedTime = (currentTime - storedTime) / 1000; // زمان سپری شده به ثانیه

        if (elapsedTime > 300) { // 300 ثانیه = 5 دقیقه
          alert("مهلت تموم شده است!");
        } else {
          // محاسبه زمان باقی‌مانده
          var remainingTime = 300 - elapsedTime; // زمان باقی‌مانده به ثانیه
           minutes = Math.floor(remainingTime / 60); // محاسبه دقیقه
           seconds = Math.floor(remainingTime % 60); // محاسبه ثانیه

          console.log("زمان باقی‌مانده: " + minutes + " دقیقه و " + seconds + " ثانیه.");
        }
      }
    }

    // فراخوانی تابع بررسی زمان
    checkCookieExpiry();
    document.addEventListener('keydown', function (event) {
      const target = event.target;
      const key = event.key;

      // بررسی اینکه آیا عنصر هدف یک input یا textarea است
      if (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA') {
        // لیست کلیدهای مجاز (Shift, Alt, Ctrl, Backspace, Delete, Arrow keys)
        const allowedKeys = ['Shift', 'Alt', 'Control', 'Backspace', 'Delete', 'ArrowLeft', 'ArrowRight'];
        
        // بررسی اینکه آیا کاراکتر وارد شده یک عدد است یا یکی از کلیدهای مجاز
        if (!/^\d$/.test(key) && !allowedKeys.includes(key)) {
          alert('فقط مجاز به ورود اعداد هستید')
          event.preventDefault(); // جلوگیری از ثبت کاراکتر غیرعددی یا غیرمجاز
        }
      }
    });
    function updateOtpValue() {
      var inputs = document.querySelectorAll('.numeral-mask');
      var otpInput = document.querySelector('#RequestViewModel_Code');
      var confirmMobile = document.querySelector('#confirmMobile');

      var otpValue = Array.from(inputs).map(input => input.value).join('');

      if (otpValue.length === 5) {
        otpInput.value = otpValue;
        document.forms[0].submit();
      } else {
        otpInput.value = '';
      }

    }

    document.addEventListener('input', updateOtpValue);


    const timerDisplay = document.getElementById("timer");


    function startTimer() {
      const timerInterval = setInterval(() => {
        timerDisplay.textContent = formatTime(minutes, seconds);

        if (seconds === 0) {
          if (minutes === 0) {
            clearInterval(timerInterval);
            timerDisplay.textContent = "زمان وارد کردن کد ورود به اتمام رسید لطفا دوباره اقدام به گرفتن کد نمایید";
            // Add your code here to perform an action when the timer reaches zero
            var userMobile = document.getElementById('RequestViewModel_PhoneNumber').value
            const confirmMobileButton = document.getElementById("confirmMobile");
            confirmMobileButton.disabled = true;
            const SendSmsAgain = document.getElementById("SendSmsAgain");
            const editPhoneNumber = document.getElementById("editPhoneNumber");
            // Remove the "disabled" class
            SendSmsAgain.classList.remove("d-none");

            // Add the "btn-primary" class
            editPhoneNumber.classList.add("d-none");

            Swal.fire({
              icon: "error",
              title: "اتمام زمان",
              text: "زمان وارد کردن کد ورود به اتمام رسید ",
              showCloseButton: false,
              showConfirmButton: false,
              showCancelButton: false,

              footer: `<a class="btn btn-outline-primary col-11"  href="LoginRegisterWithMobile?mobile=${userMobile}">دریافت دوباره کد ورود</a>`
            });
          } else {
            minutes--;
            seconds = 59;
          }
        } else {
          seconds--;
        }
      }, 1000);
    }

    function formatTime(minutes, seconds) {
      const minuteString = minutes < 10 ? `0${minutes}` : `${minutes}`;
      const secondString = seconds < 10 ? `0${seconds}` : `${seconds}`;
      return `${minuteString}:${secondString}`;
    }
    startTimer();
  })();
});
