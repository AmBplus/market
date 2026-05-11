
productPage()
function productPage() {
    productGallerySwiper()
    productSeller()
    productReview()
    productProperties()
    tabs()
    addComment()
    gallery()
    userComment()
/*    chart()*/


    function productGallerySwiper() {
        new Swiper('.productGallery-swiper', {


            pagination: {
                el: ".swiper-pagination",
                dynamicBullets: true,
                dynamicMainBullets: 3,

            }
        })
    }


    function productSeller() {
        $('.product-sellers-btnShowMore').click(function () {
            let text = $(this).children('span');
            $('.product-sellerList').toggleClass('showAll');
            text.text(text.text() == 'بستن' ? 'مشاهده بیشتر' : 'بستن')
        });


        $('.product-btn-gotoSeller').click(function () {

            $('html,body').animate({
                scrollTop: $('.product-sellerList').offset().top - header_height + 20
            }, 250)
        })
    }

    function productReview() {
        let reviewItem = $('.product-review-item');
        if (reviewItem.length <= 1) $('.product-review-btnShowmore').addClass('d-lg-none d-none');

        $('.product-review-btnShowmore.desktop').click(function () {
            let btnText = $(this).children('span');
            $('.product-review-list').toggleClass('showAll');
            btnText.text(btnText.text() == 'بستن' ? 'مشاهده بیشتر' : 'بستن')
        })

    }

    function productProperties() {

        let propertiesItems = $('.product-properties-values > div')
        if (propertiesItems.length <= 5) $('.product-properties-btnShowmore').addClass('d-lg-none d-none');

        $('.product-properties-btnShowmore.desktop').click(function () {
            let btnText = $(this).children('span');
            $('.product-properties-values').toggleClass('showAll');
            btnText.text(btnText.text() == 'بستن' ? 'مشاهده بیشتر' : 'بستن')
        })

    }


    function tabs() {
        let stickyItem_top = 0

        let resizeObserver = new ResizeObserver(() => {

            $('.product-tab-list').css('top', $('header').height());
            stickyItem_top = $('header').height() + 65;

            $('.product-tab-side-Sticky').css('top', stickyItem_top);
        });


        resizeObserver.observe($('header')[0])

        let tabs = $('.product-tabs-item'),
            content = $('.product-tabs-content-item');

        window.onscroll = function () {

            let scrollTop = win.scrollTop();
            tabs.removeClass('active');

            if (scrollTop < content.eq(0).offset().top)
                tabs.eq(0).addClass('active');
            else {
                content.each(function (i) {

                    if (content.eq(i + 1).length && scrollTop + stickyItem_top > content.eq(i + 1).offset().top) return;

                    if (scrollTop + stickyItem_top > $(this).offset().top)
                        tabs.eq(i).addClass('active');
                })

            }

        }

        tabs.click(function () {

            $('html, body').animate({
                scrollTop: content.eq(tabs.index(this)).offset().top - stickyItem_top + 20
            }, 250)
        })

    }

    function addComment() {
        let slider = document.querySelector('.addComment-rateSlider'),
            min = 0,
            max = 5;

        noUiSlider.create(slider, {
            start: min,
            step: 1,
            direction: 'rtl',
            connect: 'lower',
            range: {
                'min': min,
                'max': max
            }
        })

        let spantext = $('.product-addComment-rateValue');
        slider.noUiSlider.on('update', function (values, handle) {
            var value = parseInt(values[handle]);

            switch (value) {
                case 1:
                    spantext.text('خیلی بد');
                    break;
                case 2:
                    spantext.text('بد');
                    break;
                case 3:
                    spantext.text('معمولی');
                    break;
                case 4:
                    spantext.text('خوب');
                    break;
                case 5:
                    spantext.text('عالی');
                    break;
                default:
                    spantext.text('');
            }
        })

        $('.addComment-addPros').click(function () {
            let inputval = $('.addComment-inputpros').val();

            if (inputval.length > 2) {
                var html = '<div class="mt-2 d-flex align-items-center">'
                    + '<i class="icon-addSimple color-success-100 icon-fs-large"></i>'
                    + '<span class="me-2 text-medium">' + inputval + '</span>'
                    + '<i class="addComment-delete-ProsandCons icon-fs-large pointer icon-delete color-gray-400 me-auto"></i>'
                    + '</div>';
                $('.addComment-pros').append(html);
                $('.addComment-inputpros').val('')
            }
        })

        $('.addComment-addCons').click(function () {
            let inputval = $('.addComment-inputcons').val();

            if (inputval.length > 2) {
                var html = '<div class="mt-2 d-flex align-items-center">'
                    + '<i class="icon-removeSimple color-error-100 icon-fs-large"></i>'
                    + '<span class="me-2 text-medium">' + inputval + '</span>'
                    + '<i class="addComment-delete-ProsandCons icon-fs-large pointer icon-delete color-gray-400 me-auto"></i>'
                    + '</div>';
                $('.addComment-cons').append(html);
                $('.addComment-inputcons').val('')
            }
        })

        $('body').delegate('.addComment-delete-ProsandCons', 'click', function () {
            $(this).parent().remove()
        })
    }

    function gallery() {
        let thumb = new Swiper('.gallerythumbSwiper', {
            spaceBetween: 8,
            slidesPerView: 'auto',
            breakpoints: {
                1024: {
                    allowTouchMove: false
                }
            }

        })
        let swiper = new Swiper('.gallerySwiper', {
            loop: false,

            navigation: {
                nextEl: ".swiper-btn-next",
                prevEl: ".swiper-btn-prev",
            },
            thumbs: {
                swiper: thumb
            },

            pagination: {
                el: '.number-pagination',
                type: 'custom',
                renderCustom: function (swiper, current, totol) {
                    var faCurrent = '<span>' + convertToFaDigit(current) + '</span>';
                    var faTotal = '<span>' + convertToFaDigit(totol) + '</span>';
                    return faCurrent + "/" + faTotal;
                }
            }

        })
        $('.product-galleryList-item').click(function () {
            var id = $(this).data('id');
            swiper.slideTo(id)
        })
    }

    function userComment() {
        let comments = [
            {
                "id": "1",
                "title": "آیفون 11",
                "date": "۱۰ دی ۱۴۰۲",
                "userNmae": "محمد توحیدی پور",
                "isbuyer": false,
                "rate": 1,
                "isrecommended": "recommended",
                "text": "به همه دوستان خریدن این گوشی رو توصیه میکنم واقعأ عین همون مشخصاتی هست ک داخل دیجی کالا تبلیغ کردن",
                "files": ["1.jpg"],
                "seller": "پایاگستر شهر",
                "colorName": "سفید",
                "colorValue": "#fff",
                "like": "25",
                "dislike": "35"
            },
            {
                "id": "2",
                "title": "پارت نامبر هند",
                "date": "۱۵ مهر ۱۴۰۲",
                "userNmae": "کاربر مارکت پلیس",
                "isbuyer": true,
                "rate": 3,
                "isrecommended": "notrecommended",
                "text": "کاش مارکت پلیس ذکر میکرد پارت نامبر کجا هست! جالبه که علاوه بر اینکه پارت نامبر هند هست ساخت خود هند هم هست اولین بار بود که یه محصولی از اپل میبینم که پشت کارتنش نوشته assembled in india امیدوارم تفاوتی در کیفیت ساخت وجود نداشته باشه",
                "files": ["2.jpg", "3.jpg"],
                "seller": "دیجی",
                "colorName": "سفید",
                "colorValue": "#fff",
                "like": "15",
                "dislike": "2"
            },
            {
                "id": "3",
                "title": "قیمت از همه جا ارزونتز",
                "date": "۱۴ مرداد ۱۴۰۱",
                "userNmae": "پوریا فرزانه بازقلعه",
                "isbuyer": true,
                "rate": 5,
                "isrecommended": "no_idea",
                "text": "دوستان سلام امیدوارم وقتتون بخیر باشه هر زمانی که نظر بنده رو میخونین من این گوشی رو تاریخ ۱۴۰۱/۰۵/۱۲ ساعت ۱۰ شب ثبت سفارش زدم و ۱۴۰۱/۰۵/۱۴ صبح رسید دستم.کمتر از ۴۸ ساعت من به شخصه روی پارت نامبر توو یه بازه‌ی زمانی حساس بودم ولی با یه تحقیق مختصر متوجه شدم که همه‌ی پارت نامبر ها یکی‌ان و فقط این ذهنیت ما ایرانی هاس که فکر میکنیم پارت نامبر فلان کشور ارجعیت داره یا فرق میکنی. در حالی که اصلا اینجوری نیست و با این تفاسیر که ابرکارخانه های مطرح دنیا از اتومبیل تا لوازم خانگی همه توی چین مونتاژ میشه و گاهاً خیلی بهتر و باکیفیت تر از تولید کمپانی های شرکت مبدأ هستن. من بین ۱۲۸گیگ و ۲۵۶ مردد بودم، دیدم ۱۲۸ گیگ همین گوشی نزدیک به ۱.۵۰۰.۰۰۰ تومان گرونتر از ظرفیت ۲۵۶ گیگ هست فقط بخاطر یه پارت نامبر. و همین گوشی ، همین رنگ ،با همین ظرفیت ۴ نوع قیمت دیدم توی همین سایت دیجی کالا، که با یه سرچ ساده متوجه میشین پارت نامبر دیگه قیمتش تا ۴۴ میلیون هم هست، که قاعدتاً بازی با ذهن و فن فروش توی هر مجموعه‌س.فقط یه راهنمایی یا میشه گفت یه توصیه به کسانی که میخوان این گوشی رو خریداری کنن، حتما حتما حتما کاور و محافظ صفحه نمایش و محافظ لنز رو از همین دیجی کالا خریداری کنین. چون بیرون به قیمت خیلی گزاف و حتی چند برابر براتون میوفته.در مجموع از خریدم بسیار راضی هستم و چند ساعتی هستش که توی دستمه و دارم لذتشو میبرم.پیشنهاد میکنم به همه",
                "files": ["4.jpg", "5.jpg"],
                "seller": "تکنولایف",
                "colorName": "سفید",
                "colorValue": "#fff",
                "like": "65",
                "dislike": "2"
            },
            {
                "id": "4",
                "title": "",
                "date": "۵ مهر ۱۴۰۱",
                "userNmae": "کاربر مارکت پلیس",
                "isbuyer": true,
                "rate": 3.5,
                "isrecommended": "recommended",
                "text": "خود گوشی که نیاز به نظر دادن نداره عالیه. از مارکت پلیس راضی هستم ،گوشی پلمپ و نات اکتیو بود.قیمتش هم نسبت به فروشگاه های حضوری مناسب تر بود. از خریدم راضی هستم.",
                "files": ["6.jpg",],
                "seller": "تکنولایف",
                "colorName": "مشکی",
                "colorValue": "#000",
                "like": "34",
                "dislike": "54"
            },
            {
                "id": "5",
                "title": "عالی",
                "date": "۱۷ مهر ۱۴۰۱",
                "userNmae": "امیر حسین حاصلی",
                "isbuyer": true,
                "rate": 4.9,
                "isrecommended": "recommended",
                "text": "عالی بود زود به دستم رسید گارانتی نقره فام وپلمپ و اکبند بود ممنون از دیجی کالا",
                "files": ["7.jpg", "8.jpg", "9.jpg"],
                "seller": "مارکت پلیس",
                "colorName": "مشکی",
                "colorValue": "#000",
                "like": "25",
                "dislike": "45",
                "advantages": ["دروربین", "فیلم برداری", "باتری "],
                "disadvantages": ["واقعا هیچی "]
            },



        ]
        generateSwiper()
        function generateSwiper() {
            let swiperContent = $('.userCommentSwiper .swiper-wrapper')
            let swiperContentThumb = $('.userCommentSwiper-thumb .swiper-wrapper')

            comments.forEach(element => {
                element.files.forEach(img => {
                    let slide = $('<div>', { class: "swiper-slide", 'data-id': element.id });
                    let image = $('<img>', { class: "w-p-100 object-fit-contain", src: "assets/images/comment/" + img, height: "550px" })

                    slide.append(image)
                    swiperContent.append(slide)

                    let slidethumb = $('<div>', { class: "swiper-slide", 'data-id': element.id });
                    let imagethumb = $('<img>', { class: "w-p-100 object-fit-contain", src: "assets/images/comment/" + img, height: "50px" });

                    slidethumb.append(imagethumb)
                    swiperContentThumb.append(slidethumb)
                });
            });
        }

        let thumb = new Swiper('.userCommentSwiper-thumb', {
            spaceBetween: 8,
            slidesPerView: 'auto',


        })
        let swiper = new Swiper('.userCommentSwiper', {
            loop: false,
            initialSlide: 1,
            navigation: {
                nextEl: ".swiper-btn-next",
                prevEl: ".swiper-btn-prev",
            },
            thumbs: {
                swiper: thumb
            },
            on: {
                slideChange: function () {
                    const index_current = this.realIndex;
                    const currenctSlide = this.slides[index_current];
                    var id = currenctSlide.getAttribute('data-id');
                    var slides = $('.userCommentSwiper-thumb .swiper-slide');
                    slides.addClass('d-none');

                    $('.userCommentSwiper-thumb .swiper-slide[data-id=' + id + ']').removeClass('d-none');
                    var comment = $.grep(comments, function (obj) {
                        return obj.id === id
                    })

                    $('.userCommentContainer').html($('#CommentTemplate').tmpl(comment[0]))
                }
            }



        })

        swiper.slideTo(0)
    }

    function chart() {
        getData()

        function getData() {
            let data = {
                "Days": ["1402\/07\/04", "1402\/07\/05", "1402\/07\/06", "1402\/07\/07", "1402\/07\/08", "1402\/07\/09", "1402\/07\/10", "1402\/07\/11", "1402\/07\/12", "1402\/07\/13", "1402\/07\/14", "1402\/07\/15", "1402\/07\/16", "1402\/07\/17", "1402\/07\/18", "1402\/07\/19", "1402\/07\/20", "1402\/07\/21", "1402\/07\/22", "1402\/07\/23", "1402\/07\/24", "1402\/07\/25", "1402\/07\/26", "1402\/07\/27", "1402\/07\/28", "1402\/07\/29", "1402\/07\/30", "1402\/08\/01", "1402\/08\/02", "1402\/08\/03"],
                "Series": [{
                    "name": "\u0637\u0644\u0627\u06cc\u06cc", "data": [{ "day": 0, "price": 190000, "rrp": 190000, "isMarketable": true, "seller": null, "warranty": null }, { "day": 1, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 2, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 3, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 4, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 5, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 6, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 7, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 8, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 9, "price": 205200, "rrp": 205200, "isMarketable": true, "seller": "\u0627\u06cc\u0645\u0646 \u0634\u0628\u06a9\u0647 \u0622\u0631\u0648\u0646", "warranty": "\u0644\u0648\u06a9\u0627" }, { "day": 10, "price": 205200, "rrp": 205200, "isMarketable": true, "seller": "\u0627\u06cc\u0645\u0646 \u0634\u0628\u06a9\u0647 \u0622\u0631\u0648\u0646", "warranty": "\u0644\u0648\u06a9\u0627" },
                    { "day": 11, "price": 205200, "rrp": 205200, "isMarketable": true, "seller": "\u0627\u06cc\u0645\u0646 \u0634\u0628\u06a9\u0647 \u0622\u0631\u0648\u0646", "warranty": "\u0644\u0648\u06a9\u0627" }, { "day": 12, "price": 205200, "rrp": 205200, "isMarketable": true, "seller": "\u0627\u06cc\u0645\u0646 \u0634\u0628\u06a9\u0647 \u0622\u0631\u0648\u0646", "warranty": "\u0644\u0648\u06a9\u0627" }, { "day": 13, "price": 205200, "rrp": 205200, "isMarketable": true, "seller": "\u0627\u06cc\u0645\u0646 \u0634\u0628\u06a9\u0647 \u0622\u0631\u0648\u0646", "warranty": "\u0644\u0648\u06a9\u0627" }, { "day": 14, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 15, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 16, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 17, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" },
                    { "day": 18, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 19, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 20, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 21, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 22, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 23, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 24, "price": 164200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 25, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 26, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 27, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 28, "price": 169200, "rrp": 169200, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" },]
                },
                {
                    "name": "\u062e\u0627\u06a9\u0633\u062a\u0631\u06cc", "data": [{ "day": 0, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 1, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 2, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 3, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 4, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 5, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 6, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 7, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 8, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 9, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 10, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 11, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 12, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 13, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 14, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 15, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 16, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 17, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 18, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 19, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 20, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 21, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 22, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 23, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 24, "price": 164900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 25, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 26, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 27, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 28, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 29, "price": 169900, "rrp": 169900, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" },
                    ]
                },
                {
                    "name": "\u0646\u0642\u0631\u0647 \u0627\u06cc", "data": [{ "day": 0, "price": 190000, "rrp": 190000, "isMarketable": true, "seller": null, "warranty": null }, { "day": 1, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 2, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 3, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 4, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 5, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 6, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 7, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 8, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 9, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 10, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 11, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 12, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 13, "price": 190000, "rrp": 190000, "isMarketable": false, "seller": null, "warranty": null }, { "day": 14, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 15, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 16, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 17, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 18, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 19, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 20, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 21, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 22, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 23, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 24, "price": 165000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 25, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 26, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 27, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, { "day": 28, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646", "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)" }, {
                        "day": 29, "price": 170000, "rrp": 170000, "isMarketable": true, "seller": "\u067e\u0631\u0647\u0627\u0646",
                        "warranty": "\u062a\u0648\u0633\u0639\u0647 \u0627\u0642\u062a\u0635\u0627\u062f \u062a\u0648\u0627\u0646 \u06cc\u0627\u0633\u06cc\u0646 (\u067e\u0631\u0647\u0627\u0646)"
                    },]
                }]
            };

            am5.ready(function () {
                createDataForChart(data)
            })
        }

        function createDataForChart(data) {
            let processedData = { days: data.Days, series: {}, firstSeries: data.Series[0] },
                variantList = $('.priceChart-colorlist');

            for (let i = 0; i < data.Series.length; i++) {
                let variantValue = data.Series[i].name
                processedData.series[variantValue] = data.Series[i];
                variantList.append('<div data-value="' + variantValue + '" class="priceChart-colorItem h-8 pointer color-gray-700 ms-2 d-flex align-items-center px-2 text-strong-2 border-gray-200 radius-u ' + (i === 0 ? 'active">' : '">')
                    + '<span>' + variantValue + '</span></div>');

            }

            createaChart(processedData)
        }

        function createaChart(data) {


            let root = am5.Root.new('priceChart');

            root.setThemes([
                am5themes_Animated.new(root)
            ])

            let chart = root.container.children.push(am5xy.XYChart.new(root, {

            }));

            let Xrenderer = am5xy.AxisRendererX.new(root, {
                minGridDistance: 150
            });
            Xrenderer.labels.template.setAll({
                textAlign: "center",
                fontSize: 11,
                fontFamily: "IRANYekan",
                fill: am5.color(0x62666d)
            });
            Xrenderer.grid.template.set("visible", false)

            let categoryAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                categoryField: "date",
                renderer: Xrenderer
            }))

            let axisToolTip = categoryAxis.set('tooltip', am5.Tooltip.new(root, {
                dy: -10
            }))

            axisToolTip.get("background").set("fill", am5.color(0xf0f0f1))
            axisToolTip.get("background").set("stroke", am5.color(0xf0f0f1))
            axisToolTip.get("background").set("cornerRadius", 8)

            axisToolTip.label.setAll({
                fontSize: 11,
                fontFamily: "IRANYekan",
            })




            let yRenderer = am5xy.AxisRendererY.new(root, {
                minGridDistance: 100
            })

            let valueAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                renderer: yRenderer
            }))

            valueAxis.get("renderer").labels.template.adapters.add("text", function (label, target) {
                if (typeof label == 'undefined') return '';
                return label === '0' ? '۰' : convertToFaDigit(numberCurrency(label)) + ' تومان';
            })

            yRenderer.labels.template.setAll({
                textAlign: "center",
                fontSize: 11,
                fontFamily: "IRANYekan",
                fill: am5.color(0x62666d)
            })


            let rrp = chart.series.push(am5xy.LineSeries.new(root, {
                name: 'rrp',
                xAxis: categoryAxis,
                yAxis: valueAxis,
                valueYField: "rrp",
                categoryXField: "date",
                stroke: am5.color(0xefefef)
            }));

            rrp.strokes.template.setAll({
                strokeWidth: 3,
                strokeDasharray: [8, 5]

            });

            let series = chart.series.push(am5xy.LineSeries.new(root, {
                name: 'price',
                xAxis: categoryAxis,
                yAxis: valueAxis,
                valueYField: "price",
                categoryXField: "date"
            }));

            series.strokes.template.setAll({
                strokeWidth: 3,
                templateField: 'color'
            });



            let cursor = chart.set('cursor', am5xy.XYCursor.new(root, {

            }))

            cursor.lineY.set('visible', false);
            cursor.lineX.setAll({
                stroke: am5.color(0xf0f0f1),
                strokeWidth: 15,
                strokeDasharray: []
            });

            cursor.events.on('cursormoved', function (e) {
                let x = e.target.getPrivate("positionX");
                let dataX = categoryAxis.axisPositionToIndex(x);

                let currentData = categoryAxis.data.values[dataX];

                let discount = Math.round((currentData.rrp - currentData.price) / currentData.rrp * 100);

                let Tooltip = renderTooltip({
                    price: convertToFaDigit(numberCurrency(currentData.price)),
                    rrp: convertToFaDigit(numberCurrency(currentData.rrp)),
                    discount: discount > 0 ? convertToFaDigit(discount) : undefined,
                    isMarketable: currentData.isMarketable,
                    seller: currentData.seller,
                    warranty: currentData.warranty
                });

                $('#chartTooltip').html(Tooltip)
            })



            categoryAxis.data.setAll(ConvertData(data.firstSeries, data.days))
            series.data.setAll(ConvertData(data.firstSeries, data.days))
            rrp.data.setAll(ConvertData(data.firstSeries, data.days))


            $('.priceChart-colorItem').click(function () {
                $('.priceChart-colorItem').removeClass('active');
                $(this).addClass('active');
                let val = $(this).data('value');
                series.data.setAll(ConvertData(data.series[val], data.days))
                rrp.data.setAll(ConvertData(data.series[val], data.days))
            })

            function getTooltipTemplate(hasDiscount) {
                return '<div class="priceChart-Tooltip">'
                    + '<div class="d-flex align-items-center text-medium">'
                    + '<i class="icon-seller icon-fs-medium ms-2"></i>'
                    + '<p>{{ seller }}</p>'
                    + '</div>'
                    + '<div class="d-flex align-items-center text-medium">'
                    + '<i class="icon-guarantee icon-fs-medium ms-2"></i>'
                    + '<p> {{ warranty }}</p>'
                    + '</div>'
                    + '<div class="d-flex align-items-center">'
                    + '<span class="h4">{{ price }}</span>'
                    + '<i class="icon-toman me-2"></i>'
                    + (hasDiscount ?
                        '<div class="discount-percent me-auto color-white bg-primary-700 d-flex align-items-center justify-content-center radius-large ">'
                        + '<span class="text-strong" >{{ discount }}٪</span>'
                        + ' </div>'
                        + '</div>'
                        + '<span class="d-flex justify-content-end ms-5 text-decoration-line-through color-gray-300 text-medium  ">{{ rrp }}</span>' : '</div>')

                    + '</div>';
            }

            function renderTooltip(values) {

                if (values.isMarketable) {
                    let template = getTooltipTemplate(!!values.discount);
                    let keys = Object.keys(values)
                    for (let i = 0; i < keys.length; i++) {
                        console.log(values[keys[i]])
                        template = template.replace(new RegExp('{{ *' + keys[i] + ' *}}'), values[keys[i]])
                    }

                    return template
                }
            }
            function ConvertData(series, days) {
                let newData = [];

                for (let i = 0; i < series.data.length; i++) {
                    let jDate = moment(days[series.data[i].day], "jYYYY/jM/jD")
                    let singleData = {
                        date: new Intl.DateTimeFormat('fa-IR', { day: "numeric", month: 'short' }).format(jDate),
                        price: !!series.data[i].price ? series.data[i].price : 0,
                        rrp: !!series.data[i].rrp ? series.data[i].rrp : 0,
                        isMarketable: series.data[i].isMarketable,
                        seller: series.data[i].seller,
                        warranty: series.data[i].warranty,
                        color: {
                            stroke: series.data[i].isMarketable ? am5.color(0x0fabc6) : am5.color(0xefefef)
                        }

                    }


                    newData.push(singleData)
                }


                return newData;
            }
        }

    }
}