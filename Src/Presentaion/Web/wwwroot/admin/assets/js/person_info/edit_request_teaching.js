$(function () {
  // File Picker Settings
  fetch('/assets/i18_localize/i18_fa.json')
    .then(response => response.json())
    .then(localization => {
      // Initialize Plyr with the loaded localization
      const player = new Plyr('#player', {
        i18n: localization,
        controls: [
          'play-large', // The large play button in the center
          'restart',    // Restart playback
          'rewind',     // Rewind by the seek time (default 10 seconds)
          'play',       // Play/pause playback
          'fast-forward', // Fast forward by the seek time (default 10 seconds)
          'progress',   // The progress bar and scrubber for playback and buffering
          'current-time', // The current time of playback
          'duration',   // The full duration of the media
          'mute',       // Toggle mute
          'volume',     // Volume control
          'captions',   // Toggle captions
          'settings',   // Settings menu
          'pip',        // Picture-in-picture (currently Safari only)
          'airplay',    // Airplay (currently Safari only)
          'download',   // Show a download button with a link to either the current source
          'fullscreen'  // Toggle fullscreen
        ],
      });
    });

  const inputFile = document.getElementById('TeachingRequest_URLSampleTeachingFile');
  const divFileSample = document.getElementById('FileVideo_Sample');
  const requestFileVideoMaxSize = document.getElementById('MaxVideoFileRequestTeachingSize').value;
  const fileVideo_Sample_FileText = divFileSample.querySelector('input');;
  const warning_exceed_file_message = document.querySelector('.warning-exceed-file-message');;
  SetFileUploader(inputFile, divFileSample, fileVideo_Sample_FileText);
  function SetFileUploader(InputFile, divFileUploader ,  TextInputFileUploder) {
    divFileUploader.addEventListener('click', function () {
      InputFile.click();
    })
    inputFile.addEventListener('change', function () {
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
  const AmountCourseInput = document.getElementById('TeachingRequest_CourseAmount');
  const CourseTimeInput = document.getElementById('TeachingRequest_CourseTime');
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


  // Block Ui On Edit


})
