document.addEventListener("DOMContentLoaded", function () {
  document.getElementById('btnPreConfirmCert').addEventListener('click', function () {
    // خواندن مقادیر
    const name = document.getElementById('Profile_Name').value;
    const family = document.getElementById('Profile_Family').value;
    const nationalCode = document.getElementById('Profile_NationalCode').value;
    const gender = document.getElementById('Profile_Gender').value;

    // بررسی اینکه آیا فیلدها پر شده‌اند یا خیر
    if (!name || !family || !nationalCode || !gender) {
      Swal.fire({
        title: 'خطا',
        text: 'لطفاً تمام فیلدهای مورد نظر (نام، نام خانوادگی، کد ملی، جنسیت) را پر کنید.',
        icon: 'error',
        confirmButtonText: 'باشه',
        showCancelButton: false,
        showDenyButton: false,
        confirmButtonColor: '#3085d6',
        padding: '1em',
        customClass: {
          confirmButton: 'btn btn-instagram me-3 waves-effect waves-light',
        },
        backdrop: true,
      });
      return; // متوقف کردن عملیات
    }

    // اطلاعات فرم را در قالب HTML آماده کنید
    const formDataHtml = `
        <p class="text-danger">توجه : پس از تایید اطلاعات زیر ، امکان تغیر آن وجود ندارد</p>
        <p><strong>نام:</strong> ${name}</p>
        <p><strong>نام خانوادگی:</strong> ${family}</p>
        <p><strong>کد ملی:</strong> ${nationalCode}</p>
        <p><strong>جنسیت:</strong> ${gender === '0' ? 'آقا' : 'خانم'}</p>
    `;

    // نمایش SweetAlert
    Swal.fire({
      title: 'تایید اطلاعات',
      html: formDataHtml,
      icon: 'warning',
      confirmButtonText: 'تایید',
      cancelButtonText: 'لغو',
      showCancelButton: true,
      showDenyButton: false,
      confirmButtonColor: '#3085d6',
      padding: '1em',
      customClass: {
        confirmButton: 'btn btn-success me-3 waves-effect waves-light',
        cancelButton: 'btn btn-warning me-3 waves-effect waves-light',
      },
      backdrop: true,
    
    }).then((result) => {
      if (result.isConfirmed) {
        // اگر کاربر تایید کرد، فرم را ارسال کنید
        document.getElementById('btnSubmitCert').click();
      }
    });
  });

});
