
$(document).ready(function () {
    $('.carousel').carousel();
    loadAudioPlayer();
});
function loadAudioPlayer() {
    var i;
    var path;
    var extensoin;
    var length = $('div.song-index').last().attr('id');
    if (length) {
        for (i = 0; i <= length; i++) {
            path = $("#" + i + ".song-index").siblings('.song-path-container').children('p').html();
            extension = path.split('.').pop();
            $('#myAudio').append("<source class='waitingSong' src=" + path + " type='audio/" + extension + "'>");
        }
        $("#myAudio").trigger('load');
        $("#myAudio").trigger('play');
    }
}

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
        var waitingSongs = $('#myAudio').html();
        $('#myAudio').html("");
        $('#myAudio').append("<source class='waitingSong' src=" + path + " type='audio/" + extension + "'>");
        $('#myAudio').append(waitingSongs);
        $("#myAudio").trigger('load');
        play_audio('play');
    }
});

$('audio').on('ended', function () {
    var waitingSongs = $('#myAudio').children(':not(:first)');
    $('#myAudio').html("");
    $('#myAudio').append(waitingSongs);
    $("#myAudio").trigger('load');
    $("#myAudio").trigger('play');
});


//_________________________________________________________ Just to not forget something
$('.add-playlist-to-library').click(function () {
    alert("To DO :add playlist to list of playlists")
});