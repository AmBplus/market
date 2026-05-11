let win = $(window),
    breakpoint_lg = 1024,
    header_height;

mainMenu();
setTimeout(() => {
    header()
}, 20)

searchHeader();

textWithShowMore()
modal()
quickAddTocart()
removeByClick()
productSlider();
textTruncate()
popover()
tabs()
profile();
dropDown();
goToElement()
gotoTop()
productPriceAdvantage()

function profile() {
    profileStickyAside()
    orderSearch();
    foreignUserCheckbox();
    orderDetail_transaction()
    function profileStickyAside() {


        if ($('.profile-aside').length) {

            $('.profile-aside').theiaStickySidebar({
                // top/bottom margiin in pixels
                'additionalMarginTop': 90,
                'additionalMarginBottom': 0,

                // auto up<a href="https://www.jqueryscript.net/time-clock/">date</a> height on window resize
                'updateSidebarHeight': true,
            });
        }

    }

    function orderSearch() {
        var mobileInput = $(".profile-order-searchMobile input");
        $('.profile-order-opensearchBtn').click(function () {
            $('.profile-order-container').addClass('d-none');
            $('.profile-order-search-container').removeClass('d-none');
        });

        $('.profile-order-searchCloseBtn').click(function () {
            $('.profile-order-container').removeClass('d-none');
            $('.profile-order-search-container').addClass('d-none');
            mobileInput.val('')
        });


        mobileInput.focus(function () {
            $(this).parent().addClass("active")
        });

        mobileInput.blur(function () {
            $(this).parent().removeClass("active")
        });


        mobileInput.on('change keyup paste', function () {

            if ($(this).val().length > 0) {
                $('.profile-order-container').addClass('d-none');
                $('.profile-order-search-container').removeClass('d-none');
            }
            else {
                $('.profile-order-container').removeClass('d-none');
                $('.profile-order-search-container').addClass('d-none');
            }

        })
    }


    function foreignUserCheckbox() {
        var fUser = $('#ForeignUser'),
            iranianuser = $('#iranianUser');
        $("#ForeignUserCheckbox").change(function () {
            if (this.checked) {
                fUser.removeClass('d-none')
                iranianuser.addClass('d-none')
            }
            else {
                iranianuser.removeClass('d-none')
                fUser.addClass('d-none')
            }
        });
    }


    function orderDetail_transaction() {
        let btn = $('.orderDetail-openTransaction'),
            transactions = $('.orderDetail-Transaction')

        btn.click(function () {
            transactions.toggleClass("opened")
            btn.toggleClass("opened")
        })
    }
}

$("input[type='text'].number").on({
    keyup: function (e) {
        if (e.keyCode == 8) {
            let value = convertedValue = $(this).val()
            let i = this.selectionStart;


            if ($(this).hasClass('currency'))
                convertedValue = numberCurrency(convertedValue);

            if ($(this).hasClass('fanumber'))
                convertedValue = convertToFaDigit(convertedValue);

            $(this).val(convertedValue)


            if (value.length == convertedValue.length)
                this.selectionStart = this.selectionEnd = i;
            else if (value.length < convertedValue.length)
                this.selectionStart = this.selectionEnd = i + 1;
            else
                this.selectionStart = this.selectionEnd = i - 1;
        }
    },

    keypress: function (e) {
        var charCode = e.keyCode;

        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        let value = convertedValue = $(this).val()
        let i = this.selectionStart;


        if ($(this).hasClass('fanumber'))
            convertedValue = value = value.substr(0, i) + convertToFaDigit(e.key) + value.substr(this.selectionEnd);


        if ($(this).hasClass('currency'))
            convertedValue = numberCurrency(convertedValue);

        if ($(this).hasClass('fanumber'))
            convertedValue = convertToFaDigit(convertedValue);

        $(this).val(convertedValue);

        if (convertedValue.length > value.length)
            this.selectionStart = this.selectionEnd = i + 2;
        else
            this.selectionStart = this.selectionEnd = i + 1;

        return false;

    }
})

function numberCurrency(n) {
    n = convertToEnDigit(n)
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}


(function ($) {
    $.fn.ToCurrency = function () {
        $(this).each(function () {
            let txt;

            if ($(this).is('input')) txt = $(this).val()
            else txt = $(this).text()

            let str = numberCurrency(txt);

            if ($(this).is('input')) $(this).val(str)
            else $(this).text(str)

        })
    }
}(jQuery));

(function ($) {
    $.fn.ToFaDigit = function () {
        $(this).each(function () {
            let txt;

            if ($(this).is('input')) txt = $(this).val()
            else txt = $(this).text()

            txt = convertToFaDigit(txt);

            if ($(this).is('input')) $(this).val(txt)
            else $(this).text(txt)

        })
    }
}(jQuery));

(function ($) {
    $.fn.ToCountDown = function () {
        $(this).each(function () {
            let that = $(this),
                eventTime = that.data('date'),
                currenctTime = '1366547400000',
                leftTime = eventTime - currenctTime,
                duration = moment.duration(leftTime, 'seconds');



            setInterval(function () {
                duration = moment.duration(duration.asSeconds() - 1, 'seconds')
                let value = duration.seconds() + ' : ' + duration.minutes() + ' : ' + duration.hours()

                if (that.hasClass('fanumber'))
                    value = convertToFaDigit(value);

                that.text(value)

            }, 1000)

        })
    }
}(jQuery));

function convertToFaDigit(text) {
    var en_number = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var fa_number = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

    for (i = 0; i <= 9; i++) {

        var regex = new RegExp(en_number[i], 'g');
        text = text.toString().replace(regex, fa_number[i])

    }

    return text;
}

function convertToEnDigit(text) {
    var en_number = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var fa_number = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

    for (i = 0; i <= 9; i++) {

        var regex = new RegExp(fa_number[i], 'g');
        text = text.toString().replace(regex, en_number[i])

    }

    return text;
}

$('.currency').ToCurrency()
$('.fanumber').ToFaDigit()
$('.countDown').ToCountDown()

$(function () {
    $('.input-focus-changeborder').children($('input')).focus(function () {
        $(this).parent().addClass('focused')
    })

    $('.input-focus-changeborder').children($('input')).blur(function () {
        $(this).parent().removeClass("focused")
    });

    $('.focus-changeparentBorder').children($('input')).focus(function () {
        $(this).parent().addClass('focused')
    })

    $('.focus-changeparentBorder').children($('input')).blur(function () {
        $(this).parent().removeClass("focused")
    });



})

function removeByClick() {
    $('.removeByClick').click(function () { $(this).remove() })
}

function gotoTop() {
    $('.gotoTop').click(function () {
        $('html,body').animate({
            scrollTop: 0
        }, 250)
    })
}

function goToElement() {
    let el = $('.goto');

    $('.goto').click(function () {
        var contentId = $(this).data('target');

        $('html,body').animate({
            scrollTop: $('[data-id=' + contentId + ']').offset().top - header_height
        }, 250)
    })
}






function openHeaderOverlay() {
    $('.header-overlay').addClass('active')
    $('body').addClass('no-overflow')
}

function closeOverlay() {
    $('.overlay').removeClass('active')
    $('body').removeClass('no-overflow')
}


function header() {
    let position = win.scrollTop(),
        nav = $('.header-nav'),
        header = $('header'),
        nav_height = nav.height(),
        headerTop_height = $('.header-top').outerHeight(),
        currentPosition = 0,
        main = $('.main');
    header_height = header.height()
    main.css('padding-top', header_height)

    win.scroll(function () {
        currentPosition = win.scrollTop();


        if (currentPosition < 80) return;

        if (win.width() < breakpoint_lg) return;

        if (currentPosition > position) {

            nav.css('transform', 'translateY(-100%)');

            header.height(header_height - nav_height)
        }
        else {

            nav.css('transform', '')
            header.height(header_height)

        }

        position = currentPosition;
    })


    win.on('resize', function () {
        nav_height = nav.height();
        if (win.width() < breakpoint_lg) {
            header.height('');
            header_height = header.height()
        }
        else {
            header_height = $('.header-top').outerHeight() + nav_height
        }


        main.css('padding-top', header_height)



    })



    $('.header-profilebtn').click(function (e) {
        $(".header-profile").toggleClass("active");
        //e.stopPropagation();
    })

    $(document).on("click", function (e) {

        if ($(".header-profile").has(e.target).length === 0) {
            $(".header-profile").removeClass("active");
        }
    });
}

function searchHeader() {
    new Swiper(".searchTrends-swiper", {
        spaceBetween: 8,
        freeMode: true,
        slidesPerView: "auto",
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });

    var input = $('.header-search-input'),
        result = $('.header-search-result'),
        search = $('.search');

    win.on("click", function (e) {
        if (input.is(e.target) ||
            result.is(e.target) ||
            result.has(e.target).length) {
            search.addClass("active");
            openHeaderOverlay();
        }
        else {
            search.removeClass("active");
            if ($('.header-overlay').hasClass('active'))
                closeOverlay();
        }
    })


}


function mainMenu() {
    menuHover();
    showSubMenu();
    mobileMenu();

    function menuHover() {
        let li_hover = $('.mainMenu-hover'),
            li = $('.mainMenu > li');

        li.hover(function () {
            li_hover.css('width', $(this).width());
            li_hover.css('left', $(this).offset().left)
            li_hover.css('transform', 'scaleX(1)')
        }, function () {
            li_hover.css('transform', 'scaleX(0)')
        })



    }


    function showSubMenu() {
        let mainMenuList = $('.mainMenu-list');

        $('.mainMenu-li-hasSubMenu').hover(function () {
            mainMenuList.addClass('active');
            $('.search').removeClass('active')
            openHeaderOverlay()

        }, function () {
            mainMenuList.removeClass('active');
            closeOverlay()
        })

        $('.mainMenu-list-categoryTitle').hover(function () {
            let id = $(this).data('id');

            $('.mainMenu-list-categoryTitle').removeClass('hovered');
            $('.mainMenu-sublist-content').removeClass('active');


            $(this).addClass('hovered')
            $('.mainMenu-sublist-content[data-id=' + id + ']').addClass('active')
        })
    }

    function mobileMenu() {
        addMobileClass();

        win.on('resize', addMobileClass)
        function addMobileClass() {
            var winwidth = win.width();


            if (winwidth < breakpoint_lg) {
                $('.header-nav').css({ 'transform': "translateX(312px)" })
                setTimeout(function () {
                    $('.header-nav').addClass('is-mobile')
                    $('.header-nav').css({ 'transform': "" })


                }, 5)

            }
            else {
                $('.header-nav ').removeClass('is-mobile')

            }
        }

        $('.mobile-hamburgerMenubtn').click(function () {
            $('nav.is-mobile').addClass("active");

            $('.mobile-header-overlay').addClass('active')
        })

        $('.mobile-header-overlay').click(function () {
            $('nav.is-mobile').removeClass("active");
            closeOverlay()
        })

        $('.mainMenu-list-categoryTitle').click(function () {
            if ($(this).hasClass('active')) {
                $(this).removeClass('active')
                $(this).next('.mainMenu-sublist-menulist').removeClass('d-block')
            }
            else {
                $(this).addClass('active');
                $(this).next('.mainMenu-sublist-menulist').addClass('d-block')
            }

        })

        $('.mainMenu-sublist-item-main').click(function () {
            if ($(this).hasClass('active')) {
                $(this).removeClass('active')
                $(this).nextUntil('.mainMenu-sublist-item-main').removeClass('active')
            }
            else {
                $(this).addClass('active');
                $(this).nextUntil('.mainMenu-sublist-item-main').addClass('active')
            }

        })
    }
}

function mainSlider() {
    new Swiper(".mainSlider-swiper", {

        loop: true,
        slidesPerView: "auto",
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
        autoplay: {
            delay: 7000,
          },
        pagination: {
            el: '.swiper-pagination'
        }
    });
}


function amazingCarousel() {
    new Swiper(".amazing-carousel-swiper", {


        slidesPerView: "auto",
        spaceBetween: 2,
        freeMode: true,
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });
}


function digikalaRecommendationSwiper() {
    new Swiper(".digikalaRecommendation-swiper", {
        slidesPerView: "auto",
        freeMode: true,

    });
}
function popularBrandSwiper() {
    new Swiper(".popularBrands-swiper", {
        slidesPerView: "auto",
        spaceBetween: 2,
        freeMode: true,
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });
}
function bestSellingswiper() {
    new Swiper(".bestSelling-swiper", {
        slidesPerView: "auto",
        spaceBetween: 20,
        freeMode: true,
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });
}
function textWithShowMore() {
    let showMore = $('.textWithShowore-btn-showMore'),
        span = showMore.find('span'),
        footerdesc = $('.textWithShowore ');
    showMore.click(function () {
        span.text(span.text() == 'بستن' ? 'مشاهده بیشتر' : 'بستن');

        footerdesc.toggleClass('opened')
    })
}
function modal() {
    $('[data-modal]').click(function () {
        let id = $(this).data('modal');
        $('.modal[data-id=' + id + ']').addClass('active')
        $('.modal-overlay').addClass('active');
        $('body').addClass('no-overflow')
    });
    $('.modal-close').click(function () {
        $('.modal-overlay').removeClass('active')
        $('.modal').removeClass('active')
        $('body').removeClass('no-overflow')
    })
    $('[data-modal-mobile]').click(function () {
        let id = $(this).data('modal-mobile');
        $('.mobileModal[data-id=' + id + ']').addClass('active')
        $('.modal-overlay').addClass('active');
        $('body').addClass('no-overflow')
    });

    $('.mobile-modal-close').click(function () {
        $('.modal-overlay').removeClass('active')
        $('.mobileModal').removeClass('active')
        $('body').removeClass('no-overflow')
    })
    $('.modal-overlay').click(function () {
        $('.modal-overlay').removeClass('active')
        $('.modal').removeClass('active')
        $('.mobileModal').removeClass('active')
        $('body').removeClass('no-overflow')
    });

    $('.modal-close-currenct').click(function () {
        $(this).parents($('.modal')).removeClass('active');
    });
}

function productPriceAdvantage() {
    $('.productPriceAdvantage-content').each(function (i) {
        let transform = 0;
        var that = $(this)
        let count = that.children('div').length;
        console.log(count)
        setInterval(function () {
            transform -= 20;
            that.css({ 'transform': "translateY(" + transform + "px)", "transition-duration": "300ms" });
            setTimeout(() => {
                if ((count - 1) * -20 >= transform) {
                    that.css({ 'transform': "translateY(0px)", "transition-duration": "0ms" });
                    transform = 0
                };
            }, 300);
        }, 1500);

    })
}
function quickAddTocart() {
    $('.quickAddToCart').click(function (e) {
        e.preventDefault();

        $(this).next().removeClass("d-none");
        $(this).addClass('d-none')
    })
}
function productSlider() {
    if (!$('.productSwiper').length)
        return;

    new Swiper(".productSwiper", {
        slidesPerView: "auto",
        spaceBetween: 24,
        freeMode: true,
        navigation: {
            nextEl: ".swiper-btn-next",
            prevEl: ".swiper-btn-prev",
        },
    });
}
function textTruncate() {
    let paragraph = $('.js-text-truncate');

    paragraph.each(function () {
        var that = $(this);
        let maxWords = that.data("maxwords"),
            text = that.text(),
            showMorebtn = that.next($('.s-text-truncate-btn-showMore')),
            btnText = showMorebtn.children('span'),
            words = text.trim().split(' ');

        words = words.filter(function (w) { return w !== '' });


        if (words.length > maxWords) {
            let truncate = words.slice(0, maxWords).join(' ') + ' ...';
            that.text(truncate);
            showMorebtn.removeClass('d-none')

            showMorebtn.click(function () {
                if (btnText.text() == 'بستن') that.text(truncate)
                else that.text(text)
                btnText.text(btnText.text() == 'بستن' ? 'بیشتر' : 'بستن')
            })
        }



    })

}
function popover() {
    $('body').delegate('.popover-btn.clickable', 'click', function () {
        $(this).children($('.popover')).toggleClass('opened')
    });


    $('body').delegate('.popover-btn.hoverable', 'click', function () {
        if ($(document).width() < breakpoint_lg) $(this).children($('.popover')).toggleClass('opened')
    });

    $(document).on('click', function (e) {
        if ($('.popover-btn').has(e.target).length === 0)
            $('.popover').removeClass('opened')
    })
}
function textareaCharCount() {

    $('.calcCharCount').on('change keyup paste', function () {
        let id = $(this).attr('id'),
            maxlength = $(this).attr('maxlength'),
            length = $(this).val().length;
        $("[data-texeareaCharCount=" + id + "]").text(convertToFaDigit(maxlength + '/' + length))
    })
}
function tabs() {
    $('.tabs-item').click(function () {

        let id = $(this).data('tabid');
        let tabname = $(this).data('tabname')


        let tabsContent = $('.tabs-content[data-tabname=' + tabname + ']');

        $('.tabs-item').removeClass('active');
        tabsContent.children('.tabs-content-item').removeClass('active')

        $('.tabs-item[data-tabid=' + id + ']').addClass('active');
        tabsContent.children('.tabs-content-item[data-id=' + id + ']').addClass('active');

    })
}
function dropDown() {
    var dropdown = $('.dropDown');
    dropdown.click(function () {
        $(this).children('.dropdownList').toggleClass('active');
    });

    $(document).on("click", function (e) {
        if (dropdown.has(e.target).length === 0) {
            $('.dropdownList').removeClass('active');
        }
    });


    $('.dropdownList-item').click(function () {
        var parent = $(this).parents('.dropDown'),
            textInput = parent.children('input[type=text]'),
            hiddenInput = parent.children('input[type=hidden]'),
            text = $(this).text().trim(),
            value = $(this).prop("value")

        textInput.val(text)
        hiddenInput.val(value)

    });


    dropdown.children('input[type=text]').keyup(function () {
        var parent = $(this).parents('.dropDown'),
            contentlist = parent.children('.dropdownList').children("ul").children("li")
        var value = $(this).val();

        if (value.length > 0) {
            contentlist.filter(function () {

                var txt = $(this).text();

                if (txt.toLowerCase().indexOf(value) > -1) $(this).removeClass("d-none")
                else $(this).addClass("d-none")
            })
        }
        else {


            contentlist.filter(function () {
                $(this).removeClass('d-none')
            })
        }

    })
}
function Timer() {
    $('.timer').each(function () {
        var that = $(this)
        var eventTime = that.data('date');
        var currentTime = '1366547400000';
        var leftTime = eventTime - currentTime;//Now i am passing the left time from controller itself which handles timezone stuff (UTC), just to simply question i used harcoded values.
        var duration = moment.duration(leftTime, 'seconds');
        var interval = 1000;

        let secondEl = that.find('.timer-second'),
            minutesEl = that.find('.timer-minutes'),
            hourEl = that.find('.timer-hour');

        setInterval(function () {
            // Time Out check


            //Otherwise
            duration = moment.duration(duration.asSeconds() - 1, 'seconds');

            secondEl.text(convertToFaDigit(duration.seconds()));
            minutesEl.text(convertToFaDigit(duration.minutes()));
            hourEl.text(convertToFaDigit(duration.hours()));

        }, interval);
    });
}
