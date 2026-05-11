
'use strict';
function doPageBlock() {
  console.log('do page block start')
  $.blockUI({
    message:
      '<div class="d-flex justify-content-center"><p class="mb-0">لطفا صبر کنید ...</p> <div class="sk-wave m-0"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div> </div>',
    timeout: 1000,
    css: {
      backgroundColor: 'transparent',
      color: '#fff',
      border: '0'
    },
    overlayCSS: {
      opacity: 0.5
    }
  });
}
function doPageBlock(title) {
  console.log('do page block start')
  $.blockUI({
    message:
      `<div class="d-flex justify-content-center"><p class="mb-0">${title} ...</p> <div class="sk-wave m-0"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div> </div>`,
    timeout: 1000,
    css: {
      backgroundColor: 'transparent',
      color: '#fff',
      border: '0'
    },
    overlayCSS: {
      opacity: 0.5
    }
  });
}
function doSectionBlock(section) {
  console.log('do page block start')
  $(section).block({
    message:
      '<div class="d-flex justify-content-center"><p class="mb-0">لطفا صبر کنید ...</p> <div class="sk-wave m-0"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div> </div>',
    timeout: 1000,
    css: {
      backgroundColor: 'transparent',
      color: '#fff',
      border: '0'
    },
    overlayCSS: {
      opacity: 0.5
    }
  });
}
