$(function () {
  var captchaApiPath = $("#captchaApiPath").val();
  
  document.getElementById('captchaRefresh').addEventListener('click', function () {
    // code to execute when the element with the ID 'your-id' is clicked

    $.ajax({
      type: 'GET',
      url: captchaApiPath,
      success: function (data) {
        // code to handle successful response
        console.log('API response:', data);
        $('#Captcha_Token').val(data.token); ;
        $('#captchaImage').attr('src', 'data:image/png;base64,' + data.captchBase64Data);
      },
      error: function (error) {
        // code to handle errors
        console.error('API error:', error);
      }
    });
  });
  

})
