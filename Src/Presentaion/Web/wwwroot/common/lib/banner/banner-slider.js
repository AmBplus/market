
//function closeBanner(event) {
//  event.stopPropagation(); // جلوگیری از انتشار رویداد کلیک به لینک
//  document.getElementById('adBanner').style.display = 'none';
//}

//// اسکریپت برای اسلایدر
//const banner_sliders = document.querySelectorAll('.ad-slide');
//let currentSlideCountBanner = 0;

//function banner_showSlide(index) {
//  banner_sliders.forEach((slide, i) => {
//    slide.classList.remove('active');
//    if (i === index) {
//      slide.classList.add('active');
//    }
//  });
//}

//function nextSlide() {
//  currentSlideCountBanner = (currentSlideCountBanner + 1) % banner_sliders.length;
//  banner_showSlide(currentSlideCountBanner);
//}

//// تغییر اسلاید هر 3 ثانیه
//setInterval(nextSlide, 5000);

//// نمایش اسلاید اول
//banner_showSlide(currentSlideCountBanner);
