$(function () {
  // File Picker Settings
  const inputFile = document.getElementById('NewTeachingRequest_URLSampleTeaching');
  const divFileSample = document.getElementById('FileVideo_Sample');
  const requestFileVideoMaxSize = document.getElementById('MaxVideoFileRequestTeachingSize').value;
  const fileVideo_Sample_FileText = divFileSample.querySelector('input');;
  const warning_exceed_file_message = document.querySelector('.warning-exceed-file-message');;
  SetFileUploader(inputFile, divFileSample, fileVideo_Sample_FileText);
  function SetFileUploader(InputFile, divFileUploader ,  TextInputFileUploder) {
    divFileUploader.addEventListener('click', function () {
      InputFile.click();
    })

    inputFile.addEventListener('change', function ()
    {
      warning_exceed_file_message.innerText = "";
      if (this.files.length === 0) {
        TextInputFileUploder.value = 'فایلی انتخاب نشده';
      } else {
        if (this.files[0].size > requestFileVideoMaxSize) {
          var requestSizeInMb = requestFileVideoMaxSize / 1024 / 1024;
          var message = `حداکثر حجم مجاز برای ارسال نمونه درس ${requestSizeInMb} مگابایت است شما از حداکثر مجاز عبور کرده اید
          لطفا حجم فایل خود را کاهش دهید و دوباره فایل خود را ارسال کنید
          `
          warning_exceed_file_message.innerText = message;
          TextInputFileUploder.value = `فایل غیر مجاز انتخاب شد`;
          this.files[0] = null;
        }
        else {
          TextInputFileUploder.value = 'یک فایل انتخاب شد: ' + this.files[0].name;
        }
        
      }

    });

  }
  // Course Amount Settings
  const AmountCourseInput = document.getElementById('NewTeachingRequest_CourseAmount');
  const CourseTimeInput = document.getElementById('NewTeachingRequest_CourseTime');
  const SpanShowCurrentAmountToToman = document.getElementById('AmountCourse');
  const SpanShowCourseTime = document.getElementById('CourseTime');

  AmountCourseInput.addEventListener('input', (e) => {
    const inputValue = parseInt(e.target.value);
    SpanShowCurrentAmountToToman.textContent = `مبلغ پیشنهادی شما ${inputValue.toLocaleString('fa-IR') }  تومان است` ;
  })
  CourseTimeInput.addEventListener('input', (e) => {
    const inputValue = parseInt(e.target.value);
    SpanShowCourseTime.textContent = ` میزارن مدت آموزش پیشنهادی شما ${inputValue.toLocaleString('fa-IR')}  دقیقه می‌باشد`;
  })



})
