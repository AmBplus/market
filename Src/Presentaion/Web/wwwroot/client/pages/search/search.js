
function searchPage() {
    priceRangeSlider()

    $('.search-topCategoryList-item-showMore').click(function () {
        $(this).remove()
    });


    $('.searchFilter-prop-title').click(function () {
        $(this).parents('.search-filter-aside-filter').find('.searchFilter-props ').toggleClass('opened')
    })


    $('.searchFilter-props-searchinput').keyup(function () {
        filterSearch($(this))
    })

    $('.icon-searchFilter-searchinput').click(function () {
        let input = $(this).parent().find('.searchFilter-props-searchinput');
        input.val('')
        filterSearch(input)
    })

    function filterSearch(input) {
        let value = input.val();

        let contentList = input.parents('.searchFilter-props').children("div[data-fa]")

        if (value.length > 0) {
            input.addClass('active');
            contentList.filter(function () {

                let dataEn = this.getAttribute('data-en');
                let dataFa = this.getAttribute('data-fa');

                if (dataEn.toLowerCase().indexOf(value) > -1 || dataFa.indexOf(value) > -1)
                    $(this).removeClass("d-none")
                else $(this).addClass("d-none")


            })
        }
        else {
            input.removeClass('active');
            contentList.filter(function () {
                $(this).removeClass("d-none")
            })
        }

    }


    function priceRangeSlider() {
        let slider = document.querySelector('.rangePrice-slider'),
            min = 0,
            max = 23780000,
            inputMin = document.querySelector('#pricerange-inputmin'),
            inputMax = document.querySelector('#pricerange-inputmax');

        noUiSlider.create(slider, {
            start: [min, max],
            direction: 'rtl',
            connect: true,
            range: {
                'min': min,
                'max': max
            }
        });

        slider.noUiSlider.on('update', function (values, handle) {

            var value = values[handle];

            if (handle) {
                inputMax.value = convertToFaDigit(numberCurrency(Math.round(value)))
            }
            else {

                inputMin.value = convertToFaDigit(numberCurrency(Math.round(value)))
            }
        })
        inputMax.addEventListener('keyup', function () {

            let val = convertToEnDigit(this.value).replace(/\,/g, '');
            slider.noUiSlider.set([null, val])
        })
        inputMin.addEventListener('keyup', function () {

            let val = convertToEnDigit(this.value).replace(/\,/g, '');
            slider.noUiSlider.set([val, null])
        })
    }

    $('.search-filter-aside').theiaStickySidebar({
        'additionalMarginTop': 180,
        'additionalMarginBottom': 20,
        'updateSidebarHeight': true
    })
}

$(document).ready(function () {
    searchPage()
});
