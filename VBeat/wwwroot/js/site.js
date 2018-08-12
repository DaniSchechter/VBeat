
$(document).ready(function () {
    $('.carousel').carousel();
});


$('div.playlist-song-container').hover(function () {
    $(this).css("background-color", "black");
    $(this).children('img').attr("src", "/images/play-button.png")

});

$('div.playlist-song-container').mouseleave(function () {
    $(this).css("background-color", "transparent");
    $(this).children('img').attr("src", "/images/speaker.png")
});

$('.add-song-to-audio-player').hover(function () {
    $(this).css("cursor", "pointer");
});

$('.add-song-to-audio-player').click(function () {
    var path = $(this).siblings('.song-path-container').children('p').html();
    var extension = path.split('.').pop();
    var audio = document.getElementById("myAudio"); 
    var answer = audio.canPlayType('audio/' + extension);
    if (answer == "") {
        alert("audio player do not support this type of files");
    }
    else {
        $('#myAudio').append("<source class='runningSong' src=" + path + " type='audio/" + extension + "'>");
        $("#myAudio").trigger('load');
        play_audio('play');
    }
});

$('audio').on('ended', function () {
    $("#myAudio").trigger('load');
    $("#myAudio").trigger('play');
});


//_________________________________________________________ Just to not forget something
$('.add-playlist-to-library').click(function () {
    alert("To DO :add playlist to list of playlists")
});