
'use strict';
$(function () {
  var getAllBaseCoreApi = $(".data-api_get_all").val();
  var datatables_class_name = 'datatables-base-table-dt';
 
  var DeleteBaseCoreApi = $(".data-delete_entity").val();
  var isCheckRole = $("#isCheckRole")
  var dt_user;
  isCheckRole.change(function () {
    dt_user.draw();
  });
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
    dt_user = dt_table.DataTable({
      /*      responsive: true,*/
      processing: true,
      serverSide: true,
      scrollCollapse: true,
      scrollX: true,

      // JSON file to add data
      ajax: {
        url: getAllBaseCoreApi,
        type: 'POST',
        contentType: "application/json",
        /*     dataSrc: 'data.data',*/
        data: function (d) {
          var isChecked = $('#isCheckRole').is(':checked');
          d.isCheckRole = isChecked
          return JSON.stringify(d);
        },
      }
      ,
      columns: [
        // columns according to JSON
        { data: '' },
        { data: 'title' },
        { data: 'courseTypeTitle' },
        { data: 'createDate' },
        { data: 'updateDate' },
        { data: 'statusTitle' },
        { data: 'categoryTitle' },
        { data: 'actions' }
      ],
      columnDefs: [
        {
          // For Responsive
          className: 'control',
          searchable: false,
          orderable: false,
          responsivePriority: 2,
          targets: 0,
          render: function (data, type, full, meta) {
            return '';
          }
        },
        {
          targets: 1,
          responsivePriority: 4,
        },
        {
          targets: 2,
        },
        {
          targets: 3,
          render: function (data, type, full, meta) {
            if (data + "" === "" || data==null)
              return (`<span>نامشخص</span>`);
            const date = new Date(data);
            // Get the localized date string
            const localizedDate = date.toLocaleDateString('fa-IR', { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' });
            return (
              localizedDate
            );
          }
        },
        {
          targets: 4,
          render: function (data, type, full, meta) {
            if (data + "" === "" || data==null)
              return (`<span>نامشخص</span>`);
            const date = new Date(data);
            const localizedDate = date.toLocaleDateString('fa-IR', { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' });
            return (
              localizedDate
            );
          }
        },
        {
          targets: 5,
          render: function (data, type, full, meta) {
            const requestTypeStatusNumber = full['requestTypeStatusNumber']
            var textClass = "";
            if (requestTypeStatusNumber == 1) {
              textClass = "text-center  bg-facebook badge"
            }
            else if (requestTypeStatusNumber == 2) {
              textClass = "text-center bg-instagram badge"
            }
            else if (requestTypeStatusNumber == 3) {
              textClass = "text-center bg-success  badge"
            }
            else if (requestTypeStatusNumber == 4) {
              textClass = "text-center bg-label-hover-success  badge"
            }
            // Get the localized date string
            return (
              `<span class="${textClass} text-wrap">${data}</span>`
            );
          }
        },
        {
          // Actions
          targets: -1,
          title: 'عملیات ها',
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {

            return (
              '<div class="d-flex align-items-center mx-auto text-center align-content=center">' +
              `<a href="JudgeRequestTeaching?id=${full['id']}" class="mx-auto text-center btn btn-primary"><span class="mx-1">مشاهده</span> <i class="fa fa-eye fa-lg mx-1" aria-hidden="true"></i></a>` +
              '</div>'
            );
          }
        }
      ],
      order: [[1, 'desc']],
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
        // Adding role filter once table initialized
        this.api()
          .columns(2)
          .every(function () {
            var column = this;
            var select = $(
              '<select id="UserRole" class="form-select text-capitalize"><option value="">نوع دوره</option></select>'
            )
              .appendTo('.score-type')
              .on('change', function () {
                var val = $.fn.dataTable.util.escapeRegex($(this).val());
                column.search(val ? '^' + val + '$' : '', true, false).draw();
              });

            column
              .data()
              .unique()
              .sort()
              .each(function (d, j) {
                select.append('<option class="border border-solid" value="' + d + '">' + d + '</option>');
              });
          });
      }
    });
  }
  // Delete Record
  $(`.${datatables_class_name} tbody`).on('click', '.delete-record', function () {
    var row = dt_user.row($(this).closest('tr'));
    var id = dt_user.row($(this).closest('tr')).data().id;

    Swal.fire({
      title: "آیا مطمئنید",
      text: "در صورت حذف برای بازگردانی نیاز به هماهنگی با مدیریت دارید",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: "بله ، حذف کن",
      cancelButtonText: "نه ، منصرف شدم",
      customClass: {
        confirmButton: 'btn btn-primary me-3 waves-effect waves-light',
        cancelButton: 'btn btn-label-secondary waves-effect waves-light'
      },
      buttonsStyling: false
    })
      .then(function (result) {
        if (result.value) {


          fetch(DeleteBaseCoreApi + '?id=' + id, {
            method: "post"
          })
            .then(response => {
              if (!response.ok) {
                throw new Error(response.statusText);
              }
              else {

                row.remove().draw();
                Swal.fire({
                  icon: 'success',
                  backdrop: true,
                  timer: 2000,
                  title: "عملیات با موفقیت انجام شد",
                  text: 'رکورد حذف شد',
                  customClass: {
                    confirmButton: 'btn btn-success waves-effect waves-light'
                  }
                });
              }
            })
            .catch(error => {
              console.log(error)
            });
        }
      });
  });

  // Filter form control to default size
  // ? setTimeout used for multilingual table initialization
  setTimeout(() => {
    $('.dataTables_filter .form-control').removeClass('form-control-sm');
    $('.dataTables_length .form-select').removeClass('form-select-sm');
  }, 300);
});
//var id = dt_user.row($(this).closest('tr')).data().id;

