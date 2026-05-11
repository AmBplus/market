
// تابع تبدیل اعداد به فارسی
function convertToFaDigit(num) {
    const persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    return num.toString().replace(/\d/g, digit => persianDigits[parseInt(digit)]);
}

// تابع ایجاد المان امتیاز
function createRatingElement(rate) {
    let ratingClass = '';

    if (rate < 2) {
        ratingClass = 'bg-rating-0-2';
    } else if (rate < 3) {
        ratingClass = 'bg-rating-2-3';
    } else if (rate < 4) {
        ratingClass = 'bg-rating-3-4';
    } else {
        ratingClass = 'bg-rating-4-5';
    }

    return $('<div>', {
        class: `d-flex align-items-center ms-2 mt-2 radius-small ${ratingClass} fanumber text-subtitle-strong py-1 px-2 color-white h-5`,
        text: convertToFaDigit(rate)
    });
}

// تابع ایجاد بخش وضعیت پیشنهاد
function createRecommendationElement(isrecommended) {
    if (isrecommended === "recommended") {
        return $('<div>', {
            class: 'd-flex align-items-center gap-2 mt-2 color-rating-4-5'
        }).append(
            $('<i>', { class: 'icon-like icon-fs-small' }),
            $('<span>', { class: 'text-medium', text: ' پیشنهاد می‌کنم' })
        );
    } else if (isrecommended === "notrecommended") {
        return $('<div>', {
            class: 'd-flex align-items-center gap-2 mt-2 color-warning-200'
        }).append(
            $('<i>', { class: 'icon-disLike icon-fs-small' }),
            $('<span>', { class: 'text-medium', text: ' پیشنهاد نمی‌کنم' })
        );
    } else if (isrecommended === "no_idea") {
        return $('<div>', {
            class: 'd-flex align-items-center gap-2 mt-2 color-gray-500'
        }).append(
            $('<i>', { class: 'icon-questionExclamation icon-fs-small' }),
            $('<span>', { class: 'text-medium', text: ' مطمئن نیستم' })
        );
    }

    return $();
}

// تابع ایجاد لیست مزایا/معایب
function createListItems(items, iconClass, iconColorClass) {
    const container = $('<div>');

    items.forEach(item => {
        container.append(
            $('<div>', {
                class: 'd-flex align-items-center gap-2'
            }).append(
                $('<i>', { class: `${iconClass} ${iconColorClass} icon-fs-small` }),
                $('<span>', { class: 'text-medium', text: item })
            )
        );
    });

    return container;
}

// تابع ایجاد گالری تصاویر
function createImageGallery(files) {
    const container = $('<div>', {
        class: 'd-flex flex-wrap product-CommentItem-image'
    });

    files.forEach(file => {
        container.append(
            $('<div>', {
                class: 'my-3 ms-2'
            }).append(
                $('<img>', {
                    class: 'object-fit-contain',
                    src: `client/assets/images/comment/${file}`
                })
            )
        );
    });

    return container;
}

// تابع ایجاد بخش اطلاعات محصول
function createProductInfo(seller, colorValue, colorName) {
    return $('<div>', {
        class: 'pt-3 mt-3 border-t-gray-100 d-flex align-items-center'
    }).append(
        $('<a>', {
            class: 'd-flex gap-2 align-items-center'
        }).append(
            $('<i>', { class: 'icon-seller color-gray-700 icon-fs-medium' }),
            $('<span>', { class: 'text-subtitle color-gray-700', text: seller })
        ),
        $('<i>', { class: 'icon-dotFill icon-fs-small color-gray-200 mx-2' }),
        $('<div>', {
            class: 'product-comment-purchased-color border-gray-100 radius-circle ms-2',
            css: { 'background-color': colorValue }
        }),
        $('<span>', { class: 'text-subtitle color-gray-700', text: colorName })
    );
}

// تابع ایجاد بخش لایک/دیسلایک
function createLikeDislikeSection(like, dislike) {
    return $('<div>', {
        class: 'mt-3 mb-1 d-flex justify-content-end'
    }).append(
        $('<span>', {
            class: 'text-medium ms-lg-10 color-gray-500',
            text: 'آیا این دیدگاه مفید بود؟'
        }),
        $('<button>', {
            class: 'd-flex align-items-center gap-1 px-4 color-gray-400 text-button-1 like-btn'
        }).append(
            $('<span>', {
                class: 'fanumber',
                text: convertToFaDigit(like)
            }),
            $('<i>', { class: 'icon-like icon-fs-large' })
        ),
        $('<button>', {
            class: 'd-flex align-items-center gap-1 color-gray-400 text-button-1 dislike-btn'
        }).append(
            $('<span>', {
                class: 'fanumber',
                text: convertToFaDigit(dislike)
            }),
            $('<i>', { class: 'icon-disLike icon-fs-large' })
        )
    );
}

// تابع ایجاد پاپ اور گزارش
function createPopoverElement() {
    return $('<div>', {
        class: 'popover-btn clickable position-relative pointer'
    }).append(
        $('<i>', { class: 'icon-moreVertical color-gray-400 mt-1' }),
        $('<div>', {
            class: 'popover p-2 position-absolute'
        }).append(
            $('<div>', {
                class: 'popover-container py-2 radius-medium bg-white'
            }).append(
                $('<div>', {
                    class: 'd-flex align-items-center py-4 w-p-100 px-2'
                }).append(
                    $('<i>', { class: 'icon-flag color-primary-700' }),
                    $('<span>', {
                        class: 'text-medium-3 me-4 text-nowrap',
                        text: 'گزارش این دیدگاه'
                    })
                )
            )
        )
    );
}

// تابع اصلی برای ایجاد کامنت
function createCommentElement(commentData) {
    const {
        rate,
        title,
        date,
        userNmae,
        isbuyer,
        isrecommended,
        text,
        advantages = [],
        disadvantages = [],
        files = [],
        seller,
        colorValue,
        colorName,
        like,
        dislike
    } = commentData;

    // ایجاد المان اصلی
    const commentElement = $('<div>', {
        class: 'd-flex align-items-start py-3 border-b-gray-200'
    });

    // بخش امتیاز
    commentElement.append(createRatingElement(rate));

    // بخش محتوای اصلی
    const contentContainer = $('<div>', {
        class: 'flex-grow-1'
    });

    // عنوان (در صورت وجود)
    if (title) {
        contentContainer.append(
            $('<p>', {
                class: 'h5 color-gray-900 mb-4',
                text: title
            })
        );
    }

    // اطلاعات کاربر
    const userInfo = $('<div>', {
        class: 'd-flex align-items-center border-b-gray-100 pb-3'
    }).append(
        $('<span>', {
            class: 'text-subtitle fanumber color-gray-400',
            text: date
        }),
        $('<i>', { class: 'icon-dotFill icon-fs-small color-gray-200 mx-2' }),
        $('<span>', {
            class: 'text-subtitle color-gray-400',
            text: userNmae
        })
    );

    // نشان خریدار (در صورت وجود)
    if (isbuyer) {
        userInfo.append(
            $('<div>', {
                class: 'product-comment-buyer-badge color-success-200 text-subtitle-strong px-2 me-2 radius-u d-flex align-items-center',
                text: 'خریدار'
            })
        );
    }

    contentContainer.append(userInfo);

    // بخش پیشنهاد
    const recommendationElement = createRecommendationElement(isrecommended);
    if (recommendationElement) {
        contentContainer.append(recommendationElement);
    }

    // متن نظر
    contentContainer.append(
        $('<p>', {
            class: 'mt-3 mb-1 color-gray-900 text-medium-2',
            text: text
        })
    );

    // مزایا
    if (advantages.length > 0) {
        contentContainer.append(createListItems(
            advantages,
            'icon-addSimple',
            'color-rating-4-5'
        ));
    }

    // معایب
    if (disadvantages.length > 0) {
        contentContainer.append(createListItems(
            disadvantages,
            'icon-removeSimple',
            'color-primary-700'
        ));
    }

    // گالری تصاویر
    if (files.length > 0) {
        contentContainer.append(createImageGallery(files));
    }

    // اطلاعات محصول
    contentContainer.append(createProductInfo(seller, colorValue, colorName));

    // بخش لایک/دیسلایک
    contentContainer.append(createLikeDislikeSection(like, dislike));

    // اضافه کردن بخش محتوا به المان اصلی
    commentElement.append(contentContainer);

    // پاپ اور گزارش
    commentElement.append(createPopoverElement());

    return commentElement;
}

// استفاده از تابع
$(document).ready(function () {
    // نمونه داده برای تست
    const sampleComment = {
        rate: 4.5,
        title: "محصول عالی",
        date: "۱۴۰۲/۰۵/۱۵",
        userNmae: "کاربر نمونه",
        isbuyer: true,
        isrecommended: "recommended",
        text: "این محصول واقعا عالی است و کیفیت خوبی دارد.",
        advantages: ["کیفیت بالا", "قیمت مناسب"],
        disadvantages: ["بسته‌بندی ضعیف"],
        files: ["image1.jpg", "image2.jpg"],
        seller: "فروشگاه نمونه",
        colorValue: "#ff0000",
        colorName: "قرمز",
        like: 15,
        dislike: 2
    };

    // ایجاد المان کامنت و اضافه کردن به صفحه
    const commentElement = createCommentElement(sampleComment);
    $('#comments-container').append(commentElement);

    // اضافه کردن ایونت هندلر برای لایک/دیسلایک
    $('.like-btn, .dislike-btn').on('click', function () {
        // کد مربوط به لایک/دیسلایک
        console.log('لایک/دیسلایک کلیک شد');
    });
});