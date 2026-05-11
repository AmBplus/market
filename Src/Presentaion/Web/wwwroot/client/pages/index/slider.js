// منتظر می‌مانیم تا DOM بارگذاری شود
$(document).ready(function () {

    /**
     * تابع کمکی برای مقداردهی اولیه هر نمونه Swiper
     */
    function initializeAmbSwiper(swiperElement, prevButton, nextButton) {
        return new Swiper(swiperElement, {
            loop: false,
            spaceBetween: 12,
            navigation: {
                prevEl: prevButton,
                nextEl: nextButton,
            },
            centeredSlides: false,
            autoplay: {
                delay: 2500,
                disableOnInteraction: true,
            },
            // تنظیمات واکنش‌گرا (Breakpoints) برای نمایش بیشتر محصولات
            breakpoints: {
                0: { slidesPerView: 1.2, spaceBetween: 10  },
                378: { slidesPerView: 1.5, spaceBetween: 10 },
                420: { slidesPerView: 2.2, spaceBetween: 8 },
                576: { slidesPerView: 3, spaceBetween: 8 },
                768: { slidesPerView: 4, spaceBetween: 8 },
                992: { slidesPerView: 5, spaceBetween: 8 },
                1200: { slidesPerView: 6, spaceBetween: 8 }
            }
        });
    }

    /**
     * تابع اصلی برای پیدا کردن و راه‌اندازی تمام ماژول‌های اسلایدر
     */
    function initAmbSliders(targetNode = document) {
        const selector = '.amb-modules-slider:not(.Amb-slider-initialized)';

        let modules = [];
        if (targetNode.matches && targetNode.matches(selector)) {
            modules.push(targetNode);
        }
        if (targetNode.querySelectorAll) {
            modules = modules.concat(Array.from(targetNode.querySelectorAll(selector)));
        }

        modules.forEach(moduleElement => {
            const swiperElement = moduleElement.querySelector('.amb-modules-slider-swiper');
            const prevButton = moduleElement.querySelector('.amb-modules-slider-prev');
            const nextButton = moduleElement.querySelector('.amb-modules-slider-next');

            if (swiperElement && prevButton && nextButton) {
                initializeAmbSwiper(swiperElement, prevButton, nextButton);
                moduleElement.classList.add('Amb-slider-initialized');
            } else {
                console.error("Amb Slider module is missing required elements (swiper, prev, or next button).", moduleElement);
            }
        });
    }

    // ۱. اجرای اولیه
    initAmbSliders();

    // ۲. راه‌اندازی MutationObserver برای المان‌های پویا 
    const observerConfig = { childList: true, subtree: true };

    const observer = new MutationObserver((mutationsList, observer) => {
        for (const mutation of mutationsList) {
            if (mutation.type === 'childList') {
                mutation.addedNodes.forEach(node => {
                    if (node.nodeType === 1) {
                        initAmbSliders(node);
                    }
                });
            }
        }
    });

    observer.observe(document.body, observerConfig);
});