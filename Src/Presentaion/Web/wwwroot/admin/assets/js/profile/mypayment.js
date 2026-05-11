
'use strict';
$(function () {
  var getAllBaseCoreApi = $(".data-api_get_all").val();
  var datatables_class_name = 'datatables-base-table-dt';
  //var courseId = document.querySelector('.data-courseId').value
  //var totalCourseIncomeApi = document.querySelector('.data-totalCourseIncomeApi').value
  // Ajax Get Total Course Income


  //ajax: ({
  //  url: totalCourseIncomeApi,
  //  type: 'GET',
  //  contentType: "application/json",
  //  /*     dataSrc: 'data.data',*/
  //  data: function (d) {
  //    d.courseId = courseId
  //    return JSON.stringify(d);
  //  }
  //});

  //$.ajax({
  //  url: totalCourseIncomeApi,
  //  type: "GET",
  //  dataType: "json", // Specify the expected data type
    
  //  data: { courseId: courseId },
  //  success: function (data) {
  //    //totalPayedPrice.val = data.ext_Payed_Related;
  //    //document.getElementById('totalPayedPrice').textContent   // Displ
  //    totalPayedPrice.textContent = data.totalPayedPrice + ' تومان';
  //    // Handle successful response
  //  },
  //    /*     dataSrc: 'data.data',*/
  //  error: function (xhr, status, error) {
  //    // Handle errors
  //  }
  //});

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
        { data: 'description' },
        { data: 'amountToman' },
        { data: 'price_FinalToman' },
        { data: 'price_OldToman' },       
        { data: 'trackingCode' },
        { data: 'refNumber' },        
        { data: 'createdOn' },
        { data: 'paymentResult' }
      ],
      columnDefs: [
        {

          targets: [0, 1, 5 , 6],
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {

            return (
              `<small class="fw-bold">${data}</small>`
            );
          }
        },
       
        {
          targets: 2,
          render: function (data, type, full, meta) {
            return (
              `<small class="fw-bold">${separateNumber(data)}</small>`             
            );
          }
        },
        {
          targets: 3,
          render: function (data, type, full, meta) {
            return (
              `<small class="fw-bold">${separateNumber(data)}</small>`             
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
              `<small class="fw-bold">${localizedDate}</small>`
              
            );
          }
        },

        {
          targets: 8,
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
      
      ],

      order: [[7, 'desc']],
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
        //this.api()
        //  .columns(2)
        //  .every(function () {
        //    var column = this;
        //    var select = $(
        //      '<select id="UserRole" class="form-select text-capitalize"><option value="">نوع دوره</option></select>'
        //    )
        //      .appendTo('.score-type')
        //      .on('change', function () {
        //        var val = $.fn.dataTable.util.escapeRegex($(this).val());
        //        column.search(val ? '^' + val + '$' : '', true, false).draw();
        //      });

        //    column
        //      .data()
        //      .unique()
        //      .sort()
        //      .each(function (d, j) {
        //        select.append('<option class="border border-solid" value="' + d + '">' + d + '</option>');
        //      });
        //  });
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
