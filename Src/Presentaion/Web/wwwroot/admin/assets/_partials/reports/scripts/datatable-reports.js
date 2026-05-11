'use strict';



// datatable (jquery)
$(function () {


  var data2 = Html.Raw(Json.Serialize(Model));

  var dt_multilingual_table = $('.dt-multilingual');

  var lang = 'fa';
  if (dt_multilingual_table.length) {
    var table_language = dt_multilingual_table.DataTable({
      // ajax: assetsPath + 'json/table-datatable.json',
      data: data2,
      columns: [
        { data: 'id' },
        { data: 'name' },
        { data: 'income' },
        { data: 'courseCounts' },
        { data: 'studentsCounts' },

      ],
      pageLength: -1,
      columnDefs: [
        {
          // For Responsive
          className: 'col-1',
          orderable: true,
          targets: 0,
          searchable: false,
          render: function (data, type, full, meta) {
            return data;
  }

},
  {
    // For Responsive
    className: 'col-3',
    orderable: true,
    targets: 1,
    searchable: true
  },
  {
    // For Responsive
    className: 'col-xs-2 col-1',
    orderable: true,
    targets: 2,
    searchable: true
  },
  {
    // For Responsive
    className: 'col-xs-2 col-1',
    orderable: true,
    targets: 3,
    searchable: true
  },
  {
    // For Responsive
    className: 'col-xs-2 col-1',
    orderable: true,
    targets: 4,
    searchable: true
  },


                ],
  language: {
  url: 'assets\\datatable\\fa.json'
},
  displayLength: 5,
  dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 d-flex justify-content-center justify-content-md-end"f>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
  lengthMenu: [7, 10, 25, 50, 75, 100],
  responsive: {
  details: {
    display: $.fn.dataTable.Responsive.display.modal({
      header: function (row) {
        var data = row.data();
        return 'Details of ' + data['full_name'];
      }
    }),
    type: 'column',
    renderer: function (api, rowIdx, columns) {
      var data = $.map(columns, function (col, i) {
        return col.title !== '' // ? Do not show row in modal popup if title is blank (for check box)
          ? '<tr data-dt-row="' +
          col.rowIndex +
          '" data-dt-column="' +
          col.columnIndex +
          '">' +
          '<td>' +
          col.title +
          ':' +
          '</td> ' +
          '<td>' +
          col.data +
          '</td>' +
          '</tr>'
          : '';
      }).join('');

      return data ? $('<table class="table"/><tbody />').append(data) : false;
    }
  }
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
