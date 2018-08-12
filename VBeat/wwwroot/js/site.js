
$(document).ready(function () {
    $('.carousel').carousel();
});

$('.table-cell').hover(function () {
    $(this).find("img").show();
});

$('.table-cell').mouseleave(function () {
    $(this).children("a").children("img").hide();
});

/*TO do --------------------------------------------------------------------------*/
$(".add-song-to-playlist").click(function() {
    alert("to do - add song to playlist");
});