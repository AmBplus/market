//function datatableBuilder(options) {

//  if (!options.tableClass) {
//    console.error("❌ tableClass اجباری است");
//    return;
//  }
//  if (!options.ajax) {
//    console.error("❌ ajax اجباری است (چون تو می‌خوای کامل بدهی)");
//    return;
//  }
//  if (!options.columns) {
//    console.error("❌ columns اجباری است");
//    return;
//  }

//  const defaultConfig = {
//    pageLength: 15,
//    height: "800px",
//    searchDelay: 2000,
//    serverSide: true,
//    processing: true,
//    scrollX: true,
//    scrollCollapse: true,
//    fixedHeader: true,
//    searching: false,
//    order: [[0, "asc"]],
//    columnDefs: [],
//    lengthMenu: [[10, 15, 25, 50, 100, 10000], [10, 15, 25, 50, 100, 10000]],
//    dom:
//      '<"card-header d-flex border-top rounded-0 flex-wrap py-2"' +
//      '<"me-5 ms-n2 pe-5"f>' +
//      '<"d-flex justify-content-start justify-content-md-end align-items-baseline"<"dt-action-buttons d-flex flex-column align-items-start align-items-md-center justify-content-sm-center mb-3 mb-md-0 pt-0 gap-4 gap-sm-0 flex-sm-row"lB>>' +
//      '>t' +
//      '<"row mx-2"' +
//      '<"col-sm-12 col-md-6"i>' +
//      '<"col-sm-12 col-md-6"p>' +
//      '>',
//    buttons: [
//      {
//        extend: "collection",
//        className: "btn btn-label-primary dropdown-toggle me-2",
//        text: '<i class="fas fa-file-export me-sm-1"></i> خروجی',
//        buttons: ["print", "csv", "excel", "copy"]
//      }
//    ],
//    language: {
//      url: "\\assets\\datatable\\fa.json"
//    }
//  };

//  const config = { ...defaultConfig, ...options };
//  const dt_table = $("." + config.tableClass);
//  if (!dt_table.length) return null;

//  return dt_table.DataTable(config);
//}
