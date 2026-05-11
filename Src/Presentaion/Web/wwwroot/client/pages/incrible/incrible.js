
function incrediblePage() {
    categorySwiper()
    selectedcarousel()

    function categorySwiper() {
        new Swiper(".incredible-offer-CategorySwiper", {
            spaceBetween: 32,
            slidesPerView: "auto",
            freeMode: true,
            navigation: {
                nextEl: ".swiper-btn-next",
                prevEl: ".swiper-btn-prev",
            },
        });
    }


    function selectedcarousel() {
        new Swiper(".incredible-offer-selected-carousel-swiper", {
            slidesPerView: "auto",
            spaceBetween: 8,
            freeMode: true,
            navigation: {
                nextEl: ".swiper-btn-next",
                prevEl: ".swiper-btn-prev",
            },
        });
    }
}

