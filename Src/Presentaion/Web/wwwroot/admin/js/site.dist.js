



$(function () {

  toastr.options = {
    closeButton: true,
    progressBar: true,
    preventDuplicates: true,
    newestOnTop: true,
    showEasing: 'swing',
    hideEasing: 'linear',
    showMethod: 'fadeIn',
    hideMethod: 'fadeOut',
    showDuration: 100,
    hideDuration: 1000,
    timeOut: 2500,
    extendedTimeOut: 1000,
    rtl: true,
  };
    jalaliDatepicker.startWatch({
        zIndex: 2000,
        topSpace: 10,
        overflowSpace: 3,
        hasSecond: false,
        
    });



  window.exportFunction = async function (e, dt, node, config, exportType = 2) {
    // ۱. نمایش لودینگ با SweetAlert
    Swal.fire({
      title: 'درحال دریافت گزارش',
      text: 'لطفاً شکیبا باشید...',
      allowOutsideClick: false,
      showConfirmButton: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    try {
      const requestUrl = dt.ajax.url();
      const requestMethod = dt.settings()[0]?.ajax?.type || 'POST';

      let lastParams = dt.ajax.params();
      let p;
      if (typeof lastParams === 'string') {
        p = JSON.parse(lastParams);
      } else {
        p = JSON.parse(JSON.stringify(lastParams));
      }

      p.ExportOption = { exportType: exportType };

      const response = await fetch(requestUrl, {
        method: requestMethod,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(p)
      });

      // ۲. بررسی خطاهای احتمالی سرور
      if (!response.ok) {
        throw new Error(`خطای سرور (${response.status}): امکان دریافت فایل وجود ندارد.`);
      }

      const blob = await response.blob();

      // ۳. ساخت لینک دانلود
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;

      const disposition = response.headers.get('Content-Disposition') || '';
      const match = disposition.match(/filename="?([^";]+)"?/i);
      a.download = match ? match[1] : 'Report.xlsx';

      document.body.appendChild(a);
      a.click();

      // ۴. بستن SweetAlert (unblock) بلافاصله بعد از شروع دانلود
      Swal.close();

      // تمیزکاری
      setTimeout(() => {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      }, 100);

    } catch (err) {
      console.error(err);
      // ۵. نمایش پیام خطا با SweetAlert
      Swal.fire({
        icon: 'error',
        title: 'خطا در دریافت خروجی',
        text: err.message || 'مشکلی در برقراری ارتباط با سرور به وجود آمده است.',
        confirmButtonText: 'متوجه شدم'
      });
    }
  };

  window.LoadContent = async function LoadContent(url, target, scriptPath = '', version = 1) {
    return new Promise((resolve, reject) => {
      // Fetch the content
      fetch(url)
        .then(response =>
          response.text()
        )
        .then(data => {
          // Update the target element with the fetched data

          var section = document.getElementById(target);
          if (!section) {
            console.log('not find ' + target);
          }
          section.innerHTML = data;

          // Check if a custom script path is provided
          if (scriptPath && scriptPath.trim() !== '') {
            // Dynamically load the custom script
            loadScript(scriptPath, version)
              .then(() => {
                resolve(); // Resolve the promise after the script is loaded
              })
              .catch(error => {
                console.error('Error loading custom script:', error);
                reject(error); // Reject the promise if script fails to load
              });
          } else {
            resolve(); // Resolve the promise immediately if no script is provided
          }
        })
        .catch(error => {
          console.error('Error loading content:', error);
          reject(error); // Reject the promise with an error
        });
    });
  }
  window.loadScript = async function loadScript(scriptPath, version) {
    return new Promise((resolve, reject) => {
      const script = document.createElement('script');
      script.src = `${scriptPath}?v=${version}`;
      script.onload = () => resolve();
      script.onerror = () => reject(new Error(`Failed to load script ${scriptPath}`));
      document.head.appendChild(script);
    })
  };



})






document.addEventListener("DOMContentLoaded", function () {
  const searchInputVar = document.getElementById('navSearchInput');
  const searchIconVar = document.getElementById('NavSearchIconArea');

  function performTopMenuSearch() {
    const searchTerm = searchInputVar.value.trim();
    if (searchTerm) {
      window.location.href = `/search?SearchTitle=${encodeURIComponent(searchTerm)}`;
    }
  }
  // Bind Enter key press event to search input
  if (searchInputVar != null) {
    $('#navSearchInput').on('keypress', function (event) {
      if (event.which === 13) { // 13 is the keycode for the Enter key
        performTopMenuSearch();
      }
    });
    searchIconVar.addEventListener('click', performTopMenuSearch);
  }


  let loginButton = document.getElementById("LoginButtonTop");
  if (loginButton) {
    let path = window.location.pathname + window.location.search; // شامل کوئری‌استرینگ (مانند ?id=123)
    let newHref = loginButton.getAttribute("href") + "?ReturnUrl=" + encodeURIComponent(path);
    loginButton.setAttribute("href", newHref);
  }
});



document.addEventListener('DOMContentLoaded', function () {
  const buttons_Role = document.getElementById('fetchRolesButton');
  const rolesModal = new bootstrap.Modal(document.getElementById('rolesModal'));
  const rolesList = document.getElementById('rolesList');
  const searchInput = document.getElementById('searchInput');
  if (buttons_Role) {
    buttons_Role.addEventListener('click', function () {
      const apiUrl = '/Api/Adminstration/Roles/GetMyRoles';
      fetch(apiUrl, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        }
      })
        .then(response => {
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
          return response.json();
        })
        .then(data => {
          console.log(data.data)
          displayRoles(data.data);
          rolesModal.show();
        })
        .catch(error => {
          console.error('There was a problem with the fetch operation:', error);
        });
    });
  }

  function displayRoles(data) {
    rolesList.innerHTML = '';
    let currentActiveRole = ''; // متغیر برای ذخیره نقش فعلی

    data.forEach(role => {
      const listItem = document.createElement('li');
      listItem.className = 'list-group-item';

      // تغییر رنگ ردیف اگر isActive true باشد
      if (role.isActive) {
        listItem.style.backgroundColor = '#d4edda'; // رنگ متفاوت برای ردیف اکتیو
        currentActiveRole = `${role.roleTitle} - ${role.title}`; // ذخیره نقش فعلی
      }

      // استایل‌های هاور برای تگ li
      listItem.style.transition = 'background-color 0.3s'; // انیمیشن تغییر بک‌گراند
      listItem.onmouseover = function () {
        listItem.style.backgroundColor = '#e9ecef'; // رنگ بک‌گراند در حالت هاور
      };
      listItem.onmouseout = function () {
        if (!role.isActive) {
          listItem.style.backgroundColor = 'transparent'; // بازگشت به بک‌گراند اصلی
        }
      };

      const link = document.createElement('a');
      link.href = `/Api/Adminstration/Roles/changeroles?roledomainid=${role.id}`; // لینک به صورت متغیر
      link.textContent = `${role.title} - ${role.roleTitle}`;
      // باز کردن لینک در تب جدید
      link.className = 'text-decoration-none'; // حذف زیرخط

      // استایل‌های اینلاین برای لینک
      link.style.color = '#007bff'; // رنگ لینک
      link.style.transition = 'color 0.3s'; // انیمیشن تغییر رنگ
      link.onmouseover = function () {
        link.style.color = '#0056b3'; // رنگ لینک در حالت هاور
        link.style.textDecoration = 'underline'; // زیرخط در حالت هاور
      };
      link.onmouseout = function () {
        link.style.color = '#007bff'; // بازگشت به رنگ اصلی
        link.style.textDecoration = 'none'; // حذف زیرخط
      };

      listItem.appendChild(link);
      rolesList.appendChild(listItem);
    });

    // پر کردن مقدار نقش فعلی در modal
    document.getElementById('roleModalCurrentRole').textContent = currentActiveRole;
  }


  searchInput.addEventListener('input', function () {
    const filter = searchInput.value.toLowerCase();
    const items = rolesList.getElementsByTagName('li');

    for (let i = 0; i < items.length; i++) {
      const item = items[i];
      if (item.textContent.toLowerCase().includes(filter)) {
        item.style.display = '';
      } else {
        item.style.display = 'none';
      }
    }
  });
});

$(function () {
  window.AmbExWidgetUpload = function (widgetClass) {
    const widget = document.querySelector(widgetClass);
    const fileInput = widget.querySelector('[data-uw="file"]');
    const btnPick = widget.querySelector('[data-uw="pick"]');
    const btnReset = widget.querySelector('[data-uw="reset"]');
    const btnSend = widget.querySelector('[data-uw="send"]');
    const sendRow = widget.querySelector('[data-uw="send-row"]');
    const fileNameBox = widget.querySelector('[data-uw="filename"]');
    const fileNameTxt = widget.querySelector('[data-uw="filename-text"]');
    const badgeName = widget.querySelector('[data-uw="badge-name"]');

    let state = 'idle';

    function setState(newState, fileName = '') {
      state = newState;
      if (newState === 'selected') {
        fileNameTxt.textContent = fileName;
        fileNameBox.classList.add('AmbExWidgetHasFile');
        badgeName.textContent = fileName;
        sendRow.classList.add('AmbExWidgetVisible');
      } else {
        fileNameTxt.textContent = 'هیچ فایلی انتخاب نشده';
        fileNameBox.classList.remove('AmbExWidgetHasFile');
        badgeName.textContent = '—';
        sendRow.classList.remove('AmbExWidgetVisible');
        fileInput.value = '';
      }
    }

    btnPick.addEventListener('click', () => fileInput.click());

    fileInput.addEventListener('change', function () {
      if (this.files && this.files[0]) {
        setState('selected', this.files[0].name);
      }
    });

    btnReset.addEventListener('click', () => setState('idle'));

    btnSend.addEventListener('click', () => {
      alert('فایل آماده ارسال است:\n' + badgeName.textContent);
    });

    return { setState };
  };
  window.SetEditor = function SetEditor(id, height) {
    // اگر height ارسال نشده بود یا مقدار معتبر نبود و مثبت نبود، از 350 استفاده کن
    if (typeof height === 'undefined' || height === null || height <= 0) {
      height = 350;
    }

    $(id).summernote({
      height: height,  // اینجا از پارامتر ورودی استفاده می‌شود
      lang: "fa-IR",
      toolbar: [
        ["style", ["style"]],
        ["font", ["bold", "italic", "underline", "strikethrough", "superscript", "subscript", "clear"]],
        ["fontname", ["fontname"]],
        ["fontsize", ["fontsize"]],
        ["color", ["color"]],
        ["para", ["ul", "ol", "paragraph"]],
        ["height", ["height"]],
        ["table", ["table"]],
        ["insert", ["link", "picture", "video", "hr"]],
        ["view", ["fullscreen", "codeview", "help"]]
      ],
      callbacks: {
        onImageUpload: function (files) {
          if (!files || !files.length) return;
          Array.from(files).forEach(function (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
              $(id).summernote("insertImage", e.target.result);
            };
            reader.readAsDataURL(file);
          });
        }
      }
    });
  }



  window.updateImageFromFileInput = function updateImageFromFileInput(fileInputId, imageElementId, backButtonId) {
    const fileInput = document.getElementById(fileInputId);
    const imageElement = document.getElementById(imageElementId);
    var backButton = document.getElementById(backButtonId)
    if (backButton) {
      backButton.setAttribute('data-preimage', imageElement.src); // اضافه کردن attribute
      backButton.setAttribute('data-imgelementId', imageElementId); // اضافه کردن attribute
      backButton.setAttribute('data-fileInputId', fileInputId); // اضافه کردن attribute
      backButton.addEventListener('click', (event) => {
        var preimage = event.target.getAttribute('data-preimage');
        var imgelementId = event.target.getAttribute('data-imgelementId');
        var _fileInputId = event.target.getAttribute('data-fileInputId');
        document.getElementById(imgelementId).src = preimage;
        const _fileInput = document.getElementById(_fileInputId);
        _fileInput.value = '';
        event.target.setAttribute('disabled', 'disabled');
      });
    }
    fileInput.addEventListener('change', (event) => {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          imageElement.src = e.target.result;
        };
        reader.readAsDataURL(file);
        if (backButton && backButton.hasAttribute('disabled')) {
          backButton.removeAttribute('disabled');
        }
      }
    });
  }


})



