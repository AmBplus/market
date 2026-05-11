
'use strict';
$(function () {
  var getAllBaseCoreApi = $(".data-api_get_all").val();
  var getUpdateUserCourseAssessments = $(".data_get_update_user_course_assessments").val();
  
  var datatables_class_name = 'datatables-base-table-dt';

  //#########################################################################################

  sendAjaxRequest()

  //#########################################################################################
  function sendAjaxRequest() {
    // چک کردن اتصال اینترنت
    if (!navigator.onLine) {
      alert('No internet connection. Please check your connection and try again.');
      return;
    }

    const url = getUpdateUserCourseAssessments; // جایگزین کنید با آدرس API خود
    const data = {
      // داده‌هایی که می‌خواهید به سرور بفرستید
    };

    $.ajax({
      url: url,
      method: 'GET',
      contentType: 'application/json',
      data: data,
      dataType: 'json',
      success: function (response) {
        if (response.isSuccess) {
          console.log('Success:', response.data);
          if (response.data.length <= 0) {
            alert('سوالی برای این آزمون ثبت نشده است')
            ///
          }
        } else {
          console.error('Failed:', response.MessageSingle);
        }

      },
      error: function (jqXHR, textStatus, errorThrown) {
        console.error('Error:', textStatus, errorThrown);

      }
      ,
      complete: function () {
        console.log('request complete');
        setTimeout(hideLoadingModal, 300);
      }
    })
  }


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
        { data: 'courseTitle' },
        { data: 'examTitle' },
        { data: 'examDate' },
        { data: 'examDateEnd' },        
        { data: 'examType' },
        { data: 'assessmentItemCount' },
        { data: 'sumScore' },
        { data: 'operationTitle' }
      ],
      columnDefs: [
        {

          targets: [ 1, 2],
          searchable: false,
          //orderable: false,
          render: function (data, type, full, meta) {

            return (
              `<small class="fw-bold">${data}</small>`
            );
          }
        },
        {

          targets: [0, 5, 7, 6],
          searchable: false,
          orderable: false,
          render: function (data, type, full, meta) {

            return (
              `<small class="fw-bold">${data}</small>`
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
              `<small class="fw-bold">${localizedDate}</small>`
            );
          }
        },
        {
          targets: 4,
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
          targets:-1,
          className: 'col-2',
          orderable: false,
          render: function (data, type, full, meta) {
  
            const requestTypeStatusNumber = full['operationCode']
            const multiQuestion = full['multiQuestion']
            console.log('------s')
            console.log(data)
            console.log(requestTypeStatusNumber)
            console.log(multiQuestion)
            var textClass = "";
            var textEyes = "";
            var multiQuestionHref = "";
            var isNeedBtn = false
            //select href          
            textClass = "bg-success"
            if (multiQuestion == -1 || requestTypeStatusNumber != 1) {
              multiQuestionHref = "#"
            }
            else if (multiQuestion == 0) {
              multiQuestionHref = `/exam/course/examquestion/singleQuestion/${full['courseId']}/${full['courseStudentsId']}/${[multiQuestion]}`
            }
            else if (multiQuestion == 1) {
              multiQuestionHref = `/exam/course/examquestion/singleQuestionDisableBack/${full['courseId']}/${full['courseStudentsId']}/${[multiQuestion]}`
            }
            else if (multiQuestion == 2) {
              multiQuestionHref = `/exam/course/examquestion/singleQuestion/${full['courseId']}/${full['courseStudentsId']}/${[multiQuestion]}`
            }
            if (requestTypeStatusNumber == 10) {


              multiQuestionHref = `/exam/course/examquestion/showResultExam/${full['courseId']}/${full['courseStudentsId']}/${[multiQuestion]}`
              return (
                `<a href=${multiQuestionHref} class="mx-auto text-center btn btn-sm btn-primary" target="_blank" style="width:70px !important;" >${data} ${textEyes} </a>`
              );
            }
            else if (requestTypeStatusNumber == 3 || requestTypeStatusNumber == 8) {
              textClass = "bg-info"
            }
            else if (requestTypeStatusNumber == 11) {
              textClass = "bg-warning"
            }
            else if (requestTypeStatusNumber == 5 || requestTypeStatusNumber == 6 || requestTypeStatusNumber == 7 || requestTypeStatusNumber == 2 || requestTypeStatusNumber == 4 || requestTypeStatusNumber == 9) {
              textClass = " bg-danger"
            }

            //return (
            //  '<div class="d-flex align-items-center mx-auto text-center justify-content-center lh-lg ">' +
            //  `<a href=${multiQuestionHref} class="mx-auto text-center btn-u btn-u-xs " style="font-size: 1rem;"><span class= "${textClass} text-wrap lh-base" > ${data} ${textEyes} </span></a>` +
            //  '</div>'
            //);

            if (multiQuestionHref !== '#' ) {
              return (
                `<a href=${multiQuestionHref} class="mx-auto text-center btn btn-sm btn-success" target="_blank" style="width:70px !important;" >${data} ${textEyes} </a>`
              );
            }
            return (
              `<span  class="mx-auto text-center badge ${textClass}" >${data} ${textEyes} </a>`
            );


          }
        },
       
      ],
      order: [[3, 'desc']],
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
