/**
 * Main - Front Pages
 */
'use strict';

(function () {
  const nav = document.querySelector('.layout-navbar'),

    swiperLogos = document.getElementById('swiper-clients-logos'),
    swiperReviews = document.getElementById('swiper-reviews'),
    ReviewsPreviousBtn = document.getElementById('reviews-previous-btn'),
    ReviewsNextBtn = document.getElementById('reviews-next-btn'),
    ReviewsSliderPrev = document.querySelector('.swiper-button-prev'),
    _ReviewsSliderNext = document.getElementById('_reviews-next-btn'),
    _ReviewsSliderPrev = document.getElementById('_reviews-previous-btn'),
    ReviewsSliderNext = document.querySelector('.swiper-button-next');
  // swiper carousel
  // Customers reviews

  // -----------------------------------
  if (swiperReviews) {
    new Swiper(swiperReviews, {
      slidesPerView: 1,
      spaceBetween: 5,
      grabCursor: true,
      autoplay: {
        delay: 5000,
        disableOnInteraction: false
      },
      loop: true,
      loopAdditionalSlides: 1,
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev'
      },
      breakpoints: {
        1200: {
          slidesPerView: 3,
          spaceBetween: 26
        },
        992: {
          slidesPerView: 2,
          spaceBetween: 20
        }
      }
    });
  }
  // Reviews slider next and previous
  // -----------------------------------
  // Add click event listener to next button
  ReviewsNextBtn.addEventListener('click', function () {
    ReviewsSliderNext.click();
  });
  ReviewsPreviousBtn.addEventListener('click', function () {
    ReviewsSliderPrev.click();
  });
  _ReviewsSliderNext.addEventListener('click', function () {
    ReviewsSliderNext.click();
  });
  _ReviewsSliderPrev.addEventListener('click', function () {
    ReviewsSliderPrev.click();
  });


})();
