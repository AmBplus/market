
'use strict';
$(function () {
  var pageBlock = $('.btn-page-block');

  if (pageBlock.length) {
    pageBlock.on('click', function () {

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
    });
  }
 })
