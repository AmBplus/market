$(function () {
  ShowAmountInput('largeInput','spanAmount')
  function ShowAmountInput(amountInputId , SpanId) {
    let amountInput = document.getElementById(amountInputId);
    let SpanShowCurrentAmountToToman = document.getElementById(SpanId);
    amountInput.addEventListener('input', (e) => {
      const inputValue = parseInt(e.target.value);
      if (inputValue < 0) {
        SpanShowCurrentAmountToToman.textContent = `مبلغ وارد شده نامعتبر است`;
      }
      if (inputValue > 0) {
        SpanShowCurrentAmountToToman.textContent = `مبلغ وارد شده توسط شما ${inputValue.toLocaleString('fa-IR')}  تومان است`;
      } else {
        SpanShowCurrentAmountToToman.textContent = `مبلغ وارد شده نامعتبر است`;
      }
    })
  }
  })
