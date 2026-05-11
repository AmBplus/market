
/*  doPageBlock();*/
/*  $.unblockUI();*/
$(document).ready(function () {
  // Initialize the validator

  function doPageBlock() {
    console.log('do page block start')
    $.blockUI({
      message:
        `
<div class="d-flex justify-content-center">
        <p class="text-white text-nowrap mx-2 mt-3 fw-bold"> اطلاعات در حال ارسال می‌باشد لطفا شکیبا باشید</h5>
        </p></br>
        <div>
        <div class="spinner-border" style="width: 3rem; height: 3rem;" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
</div>
        </div> `,
      css: {
        backgroundColor: 'transparent',
        color: '#fff',
        border: '0'
      },
      overlayCSS: {
        opacity: 0.5
      }
    });
  }
  var myform = $('form');
  myform.submit(function () {
    myform.validate()
    var isvalidForm = myform.valid()
    if (isvalidForm === true) {
      doPageBlock();
    }

  })

});
