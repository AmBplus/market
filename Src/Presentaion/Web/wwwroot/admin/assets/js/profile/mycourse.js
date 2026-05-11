
'use strict';
$(function () {
  var getAllBaseCoreApi = $(".data-api_get_all").val();
  var datatables_class_name = 'datatables-base-table-dt';

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
    var dt_user = dt_table.DataTable({
      searchDelay: 500,
      processing: true,
      serverSide: true,
      scrollY: '500px',
      scrollCollapse: false,
      fixedHeader: true,
      // JSON file to add data
      ajax: {
        url: getAllBaseCoreApi,
        type: 'POST',
        contentType: "application/json",
        /*     dataSrc: 'data.data',*/
        data: function (d) {
          return JSON.stringify(d);
        },
      }
      ,
      columns: [
        // columns according to JSON
        { data: 'rowNumber' },
        { data: 'title' },
        { data: 'holdingDateStart' },
        { data: 'createdOn' },
        { data: 'action' }
      ],
      columnDefs: [
        {
          targets: [0],
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {
            return (
              `<small class="fw-bold">${data}</small>`
            );
          }
        },
        {
          // Action
          targets: 1,
          searchable: true,
          orderable: true,
          render: function (data, type, full, meta) {
            return (
              '<div class="d-flex align-items-center mx-auto text-center justify-content-center">' +
              `<a href="/course/${full['courseId']}"  target="_blank" class="mx-auto text-center text-primary text-primary-hover">${data}</a>` +
              '</div>'
            );
          }
        },
        {
          targets: 2,
          searchable: false,
          className: 'control',
          render: function (data, type, full, meta) {
            const date = new Date(data);
            // Get the localized date string
            if (data + "" === "" || data==null)
              return (`<span>نامشخص</span>`);
            const localizedDate = date.toLocaleDateString('fa-IR', { year: 'numeric', month: 'long', day: 'numeric' });
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
          // Actions
          targets: -1,
          title: 'عملیات',
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {
            return (
              `
              <div class="dropdown">
                   <!-- Small Screen Button -->
                  <button type="button" class="btn btn-facebook   btn-xs d-md-none waves-effect dropdown-toggle" data-bs-toggle="dropdown">
                      جزئیات
                  </button>

                  <!-- Medium Screen Button -->
                  <button type="button" class="btn btn-facebook  btn-sm d-none d-md-inline  waves-effect dropdown-toggle" data-bs-toggle="dropdown">
                      جزئیات
                  </button>

            

                <ul class="dropdown-menu ">
                  <li class="text-center m-2"><a class="dropdown-item  btn fw-bold btn-sm btn btn-instagram  waves-effect waves-light   d-flex justify-content-center text-center  " href="/course/${full['courseId']}"   target="_blank" >مشاهده </a></li>
                  <li class="text-center m-2"><a class="dropdown-item  btn fw-bold btn-sm btn-info waves-effect  waves-effect waves-light  d-flex justify-content-center text-center  " href="/exam/course/${full['courseId']}" target="_blank" >آزمون ها</a></li>
                  <li class="text-center m-2"><a class="dropdown-item  btn fw-bold btn-sm btn-twitter waves-effect waves-light d-flex justify-content-center text-center  " href="/personinfo/mycertificate?courseId=${full['courseId']}" target="_blank" >گواهی ها</a></li>
                </ul>
              </div>
              `
            );
          }
        }
      ],
      order: [[4, 'desc']],
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
      initComplete: function () {
      }
    });
  }

  // Filter form control to default size
  // ? setTimeout used for multilingual table initialization
  setTimeout(() => {
    $('.dataTables_filter .form-control').removeClass('form-control-sm');
    $('.dataTables_length .form-select').removeClass('form-select-sm');
  }, 300);
});

function separateNumber(num) {
  return num.toLocaleString('fa-IR');
}


