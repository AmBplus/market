const sendBtn = document.getElementById("sendOtp")
const modal = document.getElementById("otpModal")
const closeBtn = document.getElementById("closeOtp")

if(sendBtn){
  sendBtn.onclick = ()=>{
    modal.style.display="flex"
  }
}

if(closeBtn){
  closeBtn.onclick = ()=>{
    modal.style.display="none"
  }
}

const otpInputs = document.querySelectorAll(".otp")

otpInputs.forEach((input,i)=>{
  input.addEventListener("input",()=>{
    input.value=input.value.replace(/[^0-9]/g,'')
    if(input.value && otpInputs[i+1]){
      otpInputs[i+1].focus()
    }
  })
})

/* persian allowed */
const faInputs=document.querySelectorAll(".fa-allowed")

faInputs.forEach(input=>{
  input.addEventListener("input",()=>{
    input.value=input.value.replace(/[^a-zA-Z0-9\u0600-\u06FF\s]/g,'')
  })
})

/* english only inputs */
const enInputs = document.querySelectorAll(".en-only");

enInputs.forEach(input => {
  input.addEventListener("input", function(){
    const faRegex = /[آ-ی]/;
    
    if (faRegex.test(this.value)) {
      iziToast.warning({
        message: 'لطفاً انگلیسی تایپ کنید',
        position: 'topCenter',
        rtl: true
      });
    }
    
    this.value = this.value.replace(/[^a-zA-Z0-9@._]/g,'');
  });
});
