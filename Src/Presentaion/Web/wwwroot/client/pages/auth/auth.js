
function loginPage() {
    $('.changepasswordVisibility').click(function () {
        let input = $(this).prev();
        if (input.prop('type') == "password") input.prop('type', 'text');
        else input.prop('type', 'password');
        $(this).children().toggleClass('d-none')
    })
}