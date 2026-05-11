
function cartPage() {

    var swiper = new Swiper(".selectDatethumbswiper", {
        spaceBetween: 24,
        slidesPerView: "auto",
        freeMode: true,

    });
    var swiper2 = new Swiper(".selectDateswiper", {
        spaceBetween: 24,

        slidesPerView: "auto",
        thumbs: {
            swiper: swiper,
        },

    });

    $('.paymernt-paymentType-showallbtn').click(function () {
        $('.paymernt-paymentType').addClass('showall')
        $(this).remove()
    });



    $('.opendiscodeBtn').click(function () {

        $(this).parent().next().removeClass('d-none')
        $(this).remove();
    })

    $('.shipmentsummery-swiper-openbtn').click(function () {
        var span = $(this).children('span')
        $(this).toggleClass('opend');
        span.text(span.text() == 'بستن' ? 'جزئیات مرسوله' : 'بستن');

        $('.shipmentsummery-swiper').toggleClass('d-none')
    })

    new Swiper(".shipmentsummery-swiper", {
        spaceBetween: 24,
        slidesPerView: "auto",
        freeMode: true,
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });

}