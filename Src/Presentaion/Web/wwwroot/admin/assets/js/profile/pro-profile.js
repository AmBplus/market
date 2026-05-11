$(function () {
  // File Picker Settings
  var nationalCardContainer = 'UploadNationalCardContainer';
  var degreeCardContainer = 'degreeCardContainer';
  var resumeCardContainer = 'resumeCardContainer';
  SetImageUploaderSettings(nationalCardContainer)
  SetImageUploaderSettings(degreeCardContainer)
  SetImageUploaderSettings(resumeCardContainer)
  function SetImageUploaderSettings(container) {
    let uploaderContainer = document.getElementById(container);
    let btnUploadEdit = uploaderContainer.querySelector('.btn-upload-edit');
    let btnBackToFirstState = uploaderContainer.querySelector('.btn-returnToFirstState');
    let fileUploader = uploaderContainer.querySelector('.fileUpload');
    let image = uploaderContainer.querySelector('.pro-profile-image');
    let imageFirstSrc = image.src;
    btnBackToFirstState.addEventListener('click', function () {
      image.src = imageFirstSrc;
      fileUploader.value = null;
    })
      btnUploadEdit.addEventListener('click', function () {
        fileUploader.click();
      })
    fileUploader.addEventListener('change', function () {

        if (this.files.length === 0) {
          alert('فایلی انتخاب نشده');
        } else {

          let reader = new FileReader();
          reader.onload = (event) => {
            image.src = event.target.result;
          };
          reader.readAsDataURL(this.files[0]);

        }
      });
  }
})
