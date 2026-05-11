// Toast.js - Wrapper for toast libraries
const Toast = {// Core methods
  show: (options) => iziToast.show(options),
  hide: (options, toast, closedBy) => iziToast.hide(options, toast, closedBy),
  destroy: () => iziToast.destroy(),
  settings: (options) => iziToast.settings(options),
  
  // Theme methods
  info: (options) => iziToast.info(options),
  success: (options) => iziToast.success(options),
  warning: (options) => iziToast.warning(options),
  error: (options) => iziToast.error(options),
  question: (options) => iziToast.question(options),
  
  // Progress control
  progress: (options, toast, callback) => iziToast.progress(options, toast, callback),
  // Settings management
  setSetting: (ref, option, value) => iziToast.setSetting(ref, option, value),
  getSetting: (ref, option) => iziToast.getSetting(ref, option)
};

// Export for use
if (typeof module !== 'undefined' && module.exports) {
  module.exports = Toast;
}
