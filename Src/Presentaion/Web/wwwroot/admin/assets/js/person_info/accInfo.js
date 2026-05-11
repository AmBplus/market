$(function () {

  // Handle form submission
  var flag = false
  $("#saveAccInfo").on('submit', function (event) {
    var form = (this);
    event.preventDefault(); // prevent default form submission
    if ($(this).valid()) {  // check if the form is valid
      Swal.fire({
        title: "آیا مطمئنید؟",
        text: "در صورتی که حساب ثبت شده ای داشته باشید ، با این شماره حساب جایگزین خواهد شد",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: "بله ، ثبت شود",
        cancelButtonText: "نه ، منصرف شدم",
        customClass: {
          confirmButton: 'btn btn-primary me-3 waves-effect waves-light',
          cancelButton: 'btn btn-label-secondary waves-effect waves-light'
        },
        buttonsStyling: false
      })
        .then(function (result) {
          if (result.value) {
            isSubmitting = true
            form.submit();
          }
          else {
            isSubmitting = false
          }
        });
    }
    else {
      isSubmitting = false
    }
  });


})
