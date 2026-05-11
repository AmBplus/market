/**
 * Simple Alert Helper (SweetAlert2)
 * --------------------------------
 * API:
 * AlertWarning(title, text?, options?)
 *
 * ✅ title اجباری
 * ✅ text اختیاری
 * ✅ options اختیاری
 */

(function (window) {

  const AlertDefaults = {
    text: '',
    timer: 3000,
    timerProgressBar: true,
    backdrop: true,
    confirmButtonText: 'باشه',
    showCancelButton: false,
    showDenyButton: false,
    confirmButtonColor: '#3085d6',
    padding: '1em',
    customClass: {
      confirmButton: 'btn btn-instagram me-3 waves-effect waves-light',
    },
  };

  function AlertFactory(icon, title, text = '', options = {}) {
    if (!title) {
      throw new Error('Alert title is required');
    }

    Swal.fire({
      ...AlertDefaults,
      icon,
      title,
      text,
      ...options,
    });
  }

  // Global methods (Readable & Predictable)

  window.AlertError = (title, text = '', options = {}) =>
    AlertFactory('error', title, text, { timer: null, ...options });

  window.AlertWarning = (title, text = '', options = {}) =>
    AlertFactory('warning', title, text, options);

  window.AlertSuccess = (title, text = '', options = {}) =>
    AlertFactory('success', title, text, options);

  window.AlertInfo = (title, text = '', options = {}) =>
    AlertFactory('info', title, text, options);

  window.AlertQuestion = (title, text = '', options = {}) =>
    AlertFactory('question', title, text, { timer: null, ...options });

})(window);
