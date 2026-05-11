document.addEventListener('DOMContentLoaded', () => {
  console.log('DOM loaded, initializing script...');

  // Find all text and password inputs
  const inputs = document.querySelectorAll('input[type="text"], input[type="password"]');

  if (inputs.length === 0) {
    console.error('Error: No input[type="text"] or input[type="password"] elements found in the DOM.');
    return;
  }

  console.log(`Found ${inputs.length} input(s), attaching keydown listeners...`);

  // Map of Persian digits to English digits
  const persianToEnglishDigits = {
    '۰': '0',
    '۱': '1',
    '۲': '2',
    '۳': '3',
    '۴': '4',
    '۵': '5',
    '۶': '6',
    '۷': '7',
    '۸': '8',
    '۹': '9'
  };

  inputs.forEach((input, index) => {
    console.log(`Attaching listener to input #${index + 1} (id: ${input.id || 'no-id'}, type: ${input.type})`);

    input.addEventListener('keydown', (event) => {
      const key = event.key;
      console.log(`Keydown on input #${index + 1}, key: ${key}`);
      if (key == undefined)
        console.log(`Special key pressed, allowing: ${key}`);
        return;
      // Allow special keys (Tab, F1-F12, Alt, Meta, etc.)
      if (['Tab', 'F1', 'F2', 'F3', 'F4', 'F5', 'F6', 'F7', 'F8', 'F9', 'F10', 'F11', 'F12', 'Alt', 'Meta','Enter'].includes(key)) {
        console.log(`Special key pressed, allowing: ${key}`);
        return; // Let special keys pass
      }

      // Allow control keys (Backspace, Arrow keys, etc.)
      if (['Backspace', 'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown', 'Delete'].includes(key)) {
        console.log(`Control key pressed, allowing: ${key}`);
        return;
      }

      // Allow modifier keys (Shift, Control, Alt, Meta)
      if (['Shift', 'Control', 'Alt', 'Meta'].includes(key)) {
        console.log(`Modifier key pressed, allowing: ${key}`);
        return; // Let modifier keys pass without alert
      }

      // Check if the key is a Persian digit
      if (persianToEnglishDigits[key]) {
        console.log(`Persian digit detected: ${key}, converting to English: ${persianToEnglishDigits[key]}`);
        event.preventDefault(); // Prevent the Persian digit
        // Insert the English digit
        const englishDigit = persianToEnglishDigits[key];
        const start = input.selectionStart;
        const end = input.selectionEnd;
        const value = input.value;
        input.value = value.slice(0, start) + englishDigit + value.slice(end);
        // Restore cursor position
        input.setSelectionRange(start + 1, start + 1);
        return;
      }

      // Define allowed English characters (letters, numbers, some special characters)
      const englishChars = /^[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]$/;

      if (englishChars.test(key)) {
        console.log(`English character allowed: ${key}`);
      } else {
        console.log(`Non-English character blocked: ${key}`);
        alert('لطفا صفحه‌کلید خود را به انگلیسی تغییر دهید.');
        event.preventDefault(); // Prevent non-English character
      }
    });
  });
});
