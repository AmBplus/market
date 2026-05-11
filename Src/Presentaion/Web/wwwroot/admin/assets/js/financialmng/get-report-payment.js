
'use strict';
$(function () {
  var datatables_class_name = 'datatables-base-table-dt';
  var getAllBaseCoreApi = $(".data-api_get_all").val();
  var getSelect2CourseApi = $(".data-select2course_api").val();
  var getSelect2DomainApi = $(".data-select2domain_api").val();
  var dataTable;
  var courseId, domainId, trackingCode, refNumber, fullName, nationalCode, mobile, startDate, endDate;


  // Find the option with the class 'select'

  let borderColor, bodyBg, headingColor;
  if (isDarkStyle) {
    borderColor = config.colors_dark.borderColor;
    bodyBg = config.colors_dark.bodyBg;
    headingColor = config.colors_dark.headingColor;
  } else {
    borderColor = config.colors.borderColor;
    bodyBg = config.colors.bodyBg;
    headingColor = config.colors.headingColor;
  }
  // Variable declaration for table
  var dt_table = $('.' + datatables_class_name),
    select2 = $('.select2')
  var lang = 'fa';
  // Users datatable

  if (dt_table.length) {
    var tableBodyClass = '.dataTables_scroll';
    var dt_user = dt_table.DataTable({
      searchDelay: 2000,
      /*      responsive: true,*/
      processing: true,
      serverSide: true,
      scrollCollapse: true,
      scrollX: true,
      scrollY: '400px',
      scrollCollapse: true,
      fixedHeader: true,
      /*      responsive: true,*/
      initComplete: function () {
        //var api = dt_user.api();
   

      },
      drawCallback: function (settings) {
        $.unblockUI();
        /*alert('DataTables has redrawn the table');*/
      },

   

      // JSON file to add data
      ajax: {
        url: getAllBaseCoreApi,
        type: 'POST',
        contentType: "application/json",
        /*     dataSrc: 'data.data',*/
        data: function (r) {
          console.log('send request ');
          r.trackingCode = trackingCode,
            r.refNumber = refNumber,
            r.domainId = domainId,
            r.fullName = fullName,
            r.nationalCode = nationalCode,
            r.mobile = mobile,
            r.startDate = startDate,
            r.endDate = endDate,
            r.courseId = courseId// $('#txtCourseId').val()

          //document.getElementById("txtCourseId").value
          return JSON.stringify(r);
        },
        beforeSend: function () {
          doSectionBlock(tableBodyClass)
        },

      }
      ,
      columns: [
        // columns according to JSON
        { data: 'rowNumber' },
        { data: 'payer' },
        { data: 'payerPhoneNumber' },
        { data: 'description' },
        { data: 'discountPercent' },
        { data: 'amountPayed' },
        { data: 'paymentTypeDetails' },
        { data: 'registrationDate' },
        { data: 'trackingCode' },
        { data: 'refNumber' },
        { data: 'paymentResult' },
        { data: 'paymentResultMsg' },
        { data: 'isHasSharing' },
        { data: 'ibanName' },
        { data: 'iban' },
        { data: 'bankName' },
        { data: 'actions' }
      ],
      columnDefs: [
        {

          targets: [0,1, 2, 3, 4, 8, 9, 11, 13, 14, 15],
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {
            if (data === null)
              data = `-`;
            return (
              `<small class="fw-bold">${data}</small>`
            );
          }
        },


        {
          targets: 5,
          render: function (data, type, full, meta) {
            return (
              `<small class="fw-bold">${separateNumber(data)}</small>`
            );
          }
        }
        ,

        {

          targets: 6,
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {

            return (
              `<small class="fw-bold">${data}</small>`
            );
          }
        },
        {
          targets: 7,
          searchable: false,
          className: 'control',
          render: function (data, type, full, meta) {
            const date = new Date(data);
            // Get the localized date string
            if (data + "" === "" || data==null)
              return (`<span>نامشخص</span>`);
            const localizedDate = date.toLocaleDateString('fa-IR', { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' });
            return (
              `<small  class="fw-bold">${localizedDate}</small>`
            );
          }
        },

        {
          targets: 10,
          render: function (data, type, full, meta) {

            var msg = "";
            if (data === false) {
              msg = `<span class="text-danger"><svg  xmlns="http://www.w3.org/2000/svg"  width="24"  height="24"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round"  class="icon icon-tabler icons-tabler-outline icon-tabler-ban"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 12m-9 0a9 9 0 1 0 18 0a9 9 0 1 0 -18 0" /><path d="M5.7 5.7l12.6 12.6" /></svg></span>`
            }
            if (data === true) {
              msg = `<span class="text-success"><svg  xmlns="http://www.w3.org/2000/svg"  width="24"  height="24"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round"  class="icon icon-tabler icons-tabler-outline icon-tabler-circle-check"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 12m-9 0a9 9 0 1 0 18 0a9 9 0 1 0 -18 0" /><path d="M9 12l2 2l4 -4" /></svg></span>`
            }
            return (
              msg
            );
          }
        },
        {
          targets: 12,
          render: function (data, type, full, meta) {

            var msg = "";
            if (data === false) {
              msg = `<span class="text-danger"><svg  xmlns="http://www.w3.org/2000/svg"  width="24"  height="24"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round"  class="icon icon-tabler icons-tabler-outline icon-tabler-ban"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 12m-9 0a9 9 0 1 0 18 0a9 9 0 1 0 -18 0" /><path d="M5.7 5.7l12.6 12.6" /></svg></span>`
            }
            if (data === true) {
              msg = `<span class="text-success"><svg  xmlns="http://www.w3.org/2000/svg"  width="24"  height="24"  viewBox="0 0 24 24"  fill="none"  stroke="currentColor"  stroke-width="2"  stroke-linecap="round"  stroke-linejoin="round"  class="icon icon-tabler icons-tabler-outline icon-tabler-circle-check"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12 12m-9 0a9 9 0 1 0 18 0a9 9 0 1 0 -18 0" /><path d="M9 12l2 2l4 -4" /></svg></span>`
            }
            return (
              msg
            );
          }
        },


        {
          // Actions
          targets: -1,
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {
            return (
              '<div class="d-flex align-items-center mx-auto text-center justify-content-center">' +
              `` +
              '</div>'
            );
          }
        }



      ],
      lengthMenu: [
        [10,15, 25, 50, 100, 1000],
        [10,15, 25, 50, 100, 1000]
      ],
      deferRender: true,
      order: [[7, 'desc']],
      dom:
        '<"card-header d-flex border-top rounded-0 flex-wrap py-2"' +
        '<"me-5 ms-n2 pe-5"f>' +
        '<"d-flex justify-content-start justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex flex-column align-items-start align-items-md-center justify-content-sm-center mb-3 mb-md-0 pt-0 gap-4 gap-sm-0 flex-sm-row"lB>>' +
        '>t' +
        '<"row mx-2"' +
        '<"col-sm-12 col-md-6"i>' +
        '<"col-sm-12 col-md-6"p>' +
        '>'
      ,
      language: {
        url: '\\assets\\datatable\\fa.json'
      },
      // Buttons with Dropdown
      buttons: [
        {
          extend: 'collection',
          className: 'btn btn-label-primary dropdown-toggle me-2 waves-effect waves-light',
          text: '<i class="fas fa-file-export me-sm-1 m"></i> <span class="d-none d-sm-inline-block">دریافت خروجی</span>',
          buttons: [
            {
              extend: 'print',
              text: '<i class="fas fa-print me-1" ></i>Print',
              className: 'dropdown-item',
            },
            {
              extend: 'csv',
              text: '<i class="fas fa-file-alt me-1" ></i>Csv',
              className: 'dropdown-item',
              bom: true,
              format: {
                body: function (inner, coldex, rowdex) {
                  if (inner.length <= 0) return inner;
                  var el = $.parseHTML(inner);
                  var result = '';
                  $.each(el, function (index, item) {
                    if (item.classList !== undefined && item.classList.contains('user-name')) {
                      result = result + item.lastChild.firstChild.textContent;
                    } else if (item.innerText === undefined) {
                      result = result + item.textContent;
                    } else result = result + item.innerText;
                  });
                  return result;
                }
              }
            },
            {
              extend: 'excel',
              text: '<i class="fas fa-file-alt me-1"></i>Excel',
              className: 'dropdown-item',
              bom: true,
              format: {
                body: function (inner, coldex, rowdex) {
                  if (inner.length <= 0) return inner;
                  var el = $.parseHTML(inner);
                  var result = '';
                  $.each(el, function (index, item) {
                    result = result + item.innerText;
                  });
                  return result;
                }
              }
            },

            {
              extend: 'copy',
              text: '<i class="fas fa-copy me-1" ></i>Copy',
              className: 'dropdown-item',
            }
          ]
        },

      ],
     
    });
    dataTable = dt_user;
  }




  // Filter form control to default size
  // ? setTimeout used for multilingual table initialization
  setTimeout(() => {
    $('.dataTables_filter .form-control').removeClass('form-control-sm');
    $('.dataTables_length .form-select').removeClass('form-select-sm');
  }, 300);


  // Setting For DatePicker
  new mds.MdsPersianDateTimePicker(document.querySelector('#StartDate'), {
    targetTextSelector: '#StartDate',
    targetDateSelector: '#payStartDate',
    //selectedDate: new Date(currentBirthDay),
    //selectedDateToShow: new Date(currentBirthDay)
  });
  new mds.MdsPersianDateTimePicker(document.querySelector('#EndDate'), {
    targetTextSelector: '#EndDate',
    targetDateSelector: '#payEndDate',
    //selectedDate: new Date(currentBirthDay),
    //selectedDateToShow: new Date(currentBirthDay)
  });

  //set EventListener 
  //let inputs = document.querySelectorAll('.paramdatatable');

  //// Add an event listener to each input element
  //inputs.forEach(input => {
  //  input.addEventListener('change', function () {
  //    // Alert the new value of the input
  //    if (this.value.length > 0) {
  //      // ایجاد و صدا زدن رویداد change
  //      dataTable.clear().draw();
  //    }

  //  });

  //});

  //
  //set EventListener 
  //let changeinput = document.querySelectorAll('.changeserach');

  //// Add an event listener to each input element
  //changeinput.forEach(input => {
  //  input.addEventListener('input', function () {
  //    if (this.value.length >= 2) {
  //      dataTable.clear().draw();
  //      //// ایجاد و صدا زدن رویداد change
  //      //var event = new Event('change');
  //      //this.dispatchEvent(event);
  //    }
  //  });

  //});
  //

  //document.getElementById('myInput').addEventListener('input', function () {
  //  if (this.value.length === 2) {
  //    // ایجاد و صدا زدن رویداد change
  //    var event = new Event('change');
  //    this.dispatchEvent(event);
  //  }
  //});


  $("#searchBtn").on('click', function (event) {

  /*  courseId = convertStringToInt(document.getElementById("txtCourseId").value);*/
   /* domainId = convertStringToInt(document.getElementById("txtDomainId").value);*/
    trackingCode = document.getElementById("txtTrackNumber").value;
    refNumber = document.getElementById("txtRefNumber").value;
    fullName = document.getElementById("txtNameFamily").value;
    nationalCode = document.getElementById("txtNationalCode").value;
    mobile = document.getElementById("txtMobileNumber").value;
    startDate = document.getElementById("StartDate").value;
    endDate = document.getElementById("EndDate").value;

    dataTable.clear().draw();

  });



  //start select2
  $('#txtCourseId').select2({
    ajax: {
     url: function(params) {
        return `${getSelect2CourseApi}?searchQuery=${params.term}&length=10`;
      },
  
      dataType: 'json',
      
      processResults: function (data) {
        // Transforms the top-level key of the response object from 'items' to 'results'
        return {
          results: data
        };
      }
      
    }
    ,
    placeholder: 'ورود حداقل دو کاراکتر',
    minimumInputLength: 2,
    allowClear: true,
    language: {
      inputTooShort: function () {
        return 'لطفا حداقل 2 کاراکتر وارد کنید';
      }
      ,
      noResults: function () {
        return 'نتیجه ای یافت نشد';
      }
    }
  });

  $('#txtCourseId').on('select2:select', function (e) {
    // Get selected option
    var selectedOption = e.params.data;
    // Get display text and value of selected option
    courseId = selectedOption.id;
    // Perform the AJAX request
  
  });

  $('#txtCourseId').on('select2:clear', function (e) {
    // Get selected option
    courseId = 0;
    // Perform the AJAX request

  });

  //select2 domain
  $('#txtDomainId').select2({
    ajax: {
      url: function (params) {
        return `${getSelect2DomainApi}?searchQuery=${params.term}&length=10`;
      },

      dataType: 'json',
      processResults: function (data) {
        // Transforms the top-level key of the response object from 'items' to 'results'
        return {
          results: data
        };
      }

    }
    ,
    placeholder: 'ورود حداقل دو کاراکتر',
    minimumInputLength: 2,
    allowClear: true,
    language: {
      inputTooShort: function () {
        return 'لطفا حداقل 2 کاراکتر وارد کنید';
      }
      ,
      noResults: function () {
        return 'نتیجه ای یافت نشد';
      }
    }
  });

  $('#txtDomainId').on('select2:select', function (e) {
    // Get selected option
    var selectedOption = e.params.data;
    // Get display text and value of selected option
    domainId = selectedOption.id;
    // Perform the AJAX request

  });

  $('#txtDomainId').on('select2:clear', function (e) {
    // Get selected option
    domainId = 0;
    // Perform the AJAX request

  });

});

function separateNumber(num) {
  return num.toLocaleString('fa-IR');
}

function convertStringToInt(inputValue) {
  let parsedNumber;

  if (inputValue !== null && inputValue.trim() !== "") {
    parsedNumber = parseInt(inputValue);
  } else {
    parsedNumber = 0; // Default value when input is null
  }

  return parsedNumber;
}
