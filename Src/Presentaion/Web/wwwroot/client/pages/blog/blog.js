
function blogWidgetSwiper() {
    new Swiper(".blog-widget-swiper", {


        slidesPerView: "auto",
        spaceBetween: 12,
        freeMode: true,
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });
}