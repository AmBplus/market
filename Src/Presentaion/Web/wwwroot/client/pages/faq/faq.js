function faq_Page() {
    $('.faq-question-item').click(function () {
        $(this).next().toggleClass('d-none')
    })
}