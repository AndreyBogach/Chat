var pageWidth = $(window).width();
if (pageWidth > 768) {
    $('.bottom-nav .dropup').hover(
        function () {
            $(this).find('.dropdown-menu').stop(true, true).fadeIn(300);
            $('i.flaticon-up-arrow-angle', this).toggleClass("flaticon-down-arrow");
        },
        function () {
            $(this).find('.dropdown-menu').stop(true, true).fadeOut(300);
            $('i.flaticon-up-arrow-angle', this).toggleClass("flaticon-down-arrow");
        });
}

/*   change avatar   */
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imagePreview').css('background-image', 'url(' + e.target.result + ')');
            $('#imagePreview').hide();
            $('#imagePreview').fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
$("#Avatar").change(function () {
    readURL(this);
});

/**/