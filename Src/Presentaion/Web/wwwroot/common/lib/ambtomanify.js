(function () {

  // Format number with comma
  function amb_Tomanify_formatNumber(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  }

  /**
   * Convert number to Persian text.
   * If too large, return formatted number.
   */
  function amb_Tomanify_numberToPersianWords(number) {

    const units = ['', 'هزار', 'میلیون', 'میلیارد'];

    if (number === 0) return '0';

    const maxSupported = 999_999_999_999;
    if (number > maxSupported) {
      return amb_Tomanify_formatNumber(number);
    }

    let result = [];
    let unitIndex = 0;

    while (number > 0) {
      let part = number % 1000;

      if (part > 0) {

        if (unitIndex >= units.length) {
          return amb_Tomanify_formatNumber(number);
        }

        let text = part.toString();
        if (unitIndex > 0) text += ' ' + units[unitIndex];
        result.unshift(text);
      }

      number = Math.floor(number / 1000);
      unitIndex++;
    }

    return result.filter(Boolean).join(' و ');
  }

  // ------------------------------------------------------
  // ⭐ Process a single input (attach events + create result element)
  // ------------------------------------------------------
  function processInput(input) {

    // prevent double-processing
    if (input.dataset.tomanifyInitialized === "true") return;
    input.dataset.tomanifyInitialized = "true";

    const resultElement = document.createElement('small');
    resultElement.className = 'amb_Tomanify_result form-text';
    resultElement.style.display = 'block';
    resultElement.style.marginTop = '4px';
    resultElement.style.color = '#006400';

    const fieldContainer =
      input.closest('.mb-3') ||
      input.closest('.form-group') ||
      input.parentElement;

    const inputName = input.getAttribute('name');
    let validationSpan = null;

    if (fieldContainer && inputName) {
      validationSpan = fieldContainer.querySelector(`[data-valmsg-for="${inputName}"]`);
    }

    if (validationSpan) {
      validationSpan.insertAdjacentElement('afterend', resultElement);
    } else {
      input.insertAdjacentElement('afterend', resultElement);
    }

    // Input listener
    input.addEventListener('input', function () {
      this.value = this.value.replace(/^0+|[^0-9]/g, '');
      const value = this.value;

      if (!value) {
        resultElement.textContent = 'مقدار نامعتبر';
        resultElement.style.color = '#8B0000';
        return;
      }

      const rial = parseInt(value, 10);

      if (rial < 1000) {
        resultElement.textContent = '';
        resultElement.style.color = '#006400';
        return;
      }

      const toman = Math.floor(rial / 10);
      const remainingRial = rial % 10;

      let text = '';
      if (toman > 0) {
        text = amb_Tomanify_numberToPersianWords(toman) + ' تومان';
        if (remainingRial > 0)
          text += ' و ' + remainingRial + ' ریال';
      } else {
        text = amb_Tomanify_numberToPersianWords(rial) + ' ریال';
      }

      const formattedRial = amb_Tomanify_formatNumber(rial) + ' ریال';
      resultElement.textContent = text + ' (' + formattedRial + ')';
      resultElement.style.color = '#006400';
    });

    // Prevent invalid keypress
    input.addEventListener('keypress', function (event) {
      const charCode = event.charCode;

      if (charCode < 48 || charCode > 57) {
        event.preventDefault();
      }

      if (this.value === '' && charCode === 48) {
        event.preventDefault();
      }
    });
  }

  // ------------------------------------------------------
  // ⭐ Initialize existing inputs on load
  // ------------------------------------------------------
  document.querySelectorAll('.amb_Tomanify_input').forEach(processInput);

  // ------------------------------------------------------
  // ⭐ MutationObserver — auto-detect added inputs
  // ------------------------------------------------------
  const observer = new MutationObserver(mutations => {
    mutations.forEach(mutation => {

      mutation.addedNodes.forEach(node => {

        // اگر node خودش input باشد
        if (node.nodeType === 1 && node.matches('.amb_Tomanify_input')) {
          processInput(node);
        }

        // اگر داخلش input جدید باشد
        if (node.querySelectorAll) {
          node.querySelectorAll('.amb_Tomanify_input').forEach(processInput);
        }

      });

    });
  });

  observer.observe(document.body, {
    childList: true,
    subtree: true
  });

})();
