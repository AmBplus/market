
'use strict';
$(function () {
  var getAllBaseCoreApi = $(".data-api_get_all").val();
  var datatables_class_name = 'datatables-base-table-dt';
  var courseId = document.querySelector('.data-courseId').value
  var getSelect2CourseApi = $(".data-select2course_api").val();
  var getSelect2DomainApi = $(".data-select2domain_api").val();
  var dataTable;
  var courseIdsearch, domainId,   fullName, nationalCode, mobile, startDate, endDate;


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
      /*      responsive: true,*/
      searchDelay: 2000,
      /*      responsive: true,*/
      processing: true,
      serverSide: true,
      scrollCollapse: true,
      scrollX: true,
      scrollY: '350px',
      scrollCollapse: true,
      fixedHeader: true,
      initComplete: function () {
        //var api = dt_user.api();
        $.unblockUI();
        //$('.dataTables_filter input').unbind('.DT').bind('keyup.DT', function (e) {
        //  var value = this.value;

        //  clearTimeout(timer);

        //  timer = setTimeout(function () {
        //    api.search(value).draw();
        //  }, 2000);
        //});
      },
      // JSON file to add data
      ajax: {
        url: getAllBaseCoreApi,
        type: 'POST',
        contentType: "application/json",
        /*     dataSrc: 'data.data',*/
        data: function (d) {
          d.courseId = courseId,
          d.domainId = domainId,
          d.fullName = fullName,
          d.nationalCode = nationalCode,
          d.mobile = mobile,
          d.startDate = startDate,
          d.endDate = endDate,
          d.courseIdsearch = courseIdsearch,// $('#txtCourseId').val()
          d.showAll = true
          return JSON.stringify(d);
        },
        beforeSend: function () {
          // Here, manually add the loading message.
          /*     doPageBlock()*/
          /*  $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);*/
          /* doPageBlock()*/
          doSectionBlock(tableBodyClass)
        },
      }
      ,
      columns: [
        // columns according to JSON
        { data: 'title' },
        { data: 'statusTitle' },
        { data: 'startDate' },
        { data: 'endDate' },
        { data: 'amount' },
        { data: 'createdOn' },
        { data: 'updateOn' },
        { data: 'description' },
        { data: 'courseName' },
        { data: 'nameFamily' },
        { data: 'action' }
      ],
      columnDefs: [
        {

          targets: [0, 7, 8, 9],
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {

            return (
              `<small class="fw-bold">${data}</small>`
            );
          }
        },

        {
          targets: 1,
          render: function (data, type, full, meta) {
            const requestTypeStatusNumber = full['status']
            var textClass = "";
            if (requestTypeStatusNumber == 70000001) {
              textClass = "text-center  bg-facebook badge"
            }
            else if (requestTypeStatusNumber == 70000003) {
              textClass = "text-center bg-danger badge"
            }
            else if (requestTypeStatusNumber == 70000004) {
              textClass = "text-center bg-success  badge"
            }
            else if (requestTypeStatusNumber == 70000002) {
              textClass = "text-center bg-warning  badge"
            }
            // Get the localized date string
            return (

              `<span class="${textClass} text-wrap">${data}</span>`
            );
          }
        },
        {
          targets: 4,
          render: function (data, type, full, meta) {
            return (

              `<small class="fw-bold">${separateNumber(data)}</small>`

            );
          }
        }
        ,
        {
          targets: 2,
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
          targets: 3,
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
          targets: 5,
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
          targets: 6,
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
          // Actions
          targets: -1,
          title: 'عملیات',
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {
            return (
              `<a class="btn btn-instagram btn-sm" href="SettlementRequestEdit/${full['id']}">مشاهده</a>`
            );
          }
        }
      ],
      lengthMenu: [
        [10,15, 25, 50, 100, 1000],
        [10,15, 25, 50, 100, 1000]
      ],
      order: [[5, 'desc']],
      dom:
        '<"row me-2"' +
        '<"col-md-2"<"me-3"l>>' +
        '<"col-md-10"<"dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-end flex-md-row flex-column mb-3 mb-md-0"fB>>' +
        '>t' +
        '<"row mx-2"' +
        '<"col-sm-12 col-md-6"i>' +
        '<"col-sm-12 col-md-6"p>' +
        '>',
      language: {
        url: '\\assets\\datatable\\fa.json'
      },
      // Buttons with Dropdown
      buttons: [

      ],
      // For responsive popup


    });


    $("#searchBtn").on('click', function (event) {

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
        url: function (params) {
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
      courseIdsearch = selectedOption.id;
      // Perform the AJAX request

    });

    $('#txtCourseId').on('select2:clear', function (e) {
      // Get selected option
      courseIdsearch = 0;
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
  }
  dataTable = dt_user;

  // Filter form control to default size
  // ? setTimeout used for multilingual table initialization
  setTimeout(() => {
    $('.dataTables_filter .form-control').removeClass('form-control-sm');
    $('.dataTables_length .form-select').removeClass('form-select-sm');
  }, 300);
});
//var id = dt_user.row($(this).closest('tr')).data().id;


function separateNumber(num) {
  return num.toLocaleString('fa-IR');
}

