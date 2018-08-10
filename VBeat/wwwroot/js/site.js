
$(document).ready(function () {
    var carousel = $("#carousel").waterwheelCarousel({
        flankingItems: 4,
    });

    //Displays first image details
    var firstImageClass = $('div#carousel img').first().attr('class');
    $('div#song-details-container div.'+firstImageClass).show();
});