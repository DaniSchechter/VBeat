
$(document).ready(function () {
    $('.carousel').carousel();
    loadAudioPlayer();
});

//--------------Show plus icon on the Home Page images------------------------- 
$('.table-cell').hover(function () {
    $(this).find("img").show();
});

$('.table-cell').mouseleave(function () {
    $(this).children("a").children("img").hide();
});

$(".add-song-to-playlist").click(function() {
    alert("to do - add song to playlist");
    loadAudioPlayer();
});


//--------------Change colors when hovering over details of song/Playlist etc..
$('div.playlist-song-container').mouseover(function () {
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

//--------------Auto play Songs on Playlist-----------------------------------------
function loadAudioPlayer() {
    var i;
    var path;
    var extensoin;
    var length = $('#playlist-song-list').children("li").last().attr('id');
    if (length) {
        //load data to the audio player
        for (i = 0; i <= length; i++) {
            path = $("li#" + i).find('.song-path-container').children('p').html();
            extension = path.split('.').pop();
            if (checkSongValidation(path))
                $('#myAudio').append("<source id=" + i + " class='waitingSong' src=" + path + " type='audio/" + extension + "'>");
            else {
                $("li#" + i).children('.playlist-song-container').css("background-color", "red");
                $("li#" + i).children('.playlist-song-container').off('mouseover');
                $("li#" + i).children('.playlist-song-container').off('mouseleave');
                $("li#" + i).find('.song-artist').html('<p style="font-weight:bold;color:black">Type Not Supported !</p>');
            }
                
                
        }
        $("#myAudio").trigger('load');
        $("#myAudio").trigger('play');
        //load the first song name
        var firstSongName = $('.song-name').children('p').first().html();
        $('#song-in-audio-name').children('p').html(firstSongName);
    }
}

//------------------Song Validation before playing on audio ------------------- 
function checkSongValidation(path) {
    var extension = path.split('.').pop();
    var audio = document.getElementById("myAudio");
    var answer = audio.canPlayType('audio/' + extension);
    return answer;
}


//------------------Add song manually to audio player on click-----------------------
$('.add-song-to-audio-player').click(function () {
    var path = $(this).siblings('.song-path-container').children('p').html();
    valid = checkSongValidation(path);
    if (valid == "") {
        alert("audio player do not support this type of files");
    }
    else {      
        //get the waiting songs in the audio player
        var waitingSongs = $('#myAudio').html();

        //get the song name and update the p
        var songId = $(this).closest('li').attr('id');
        var songName = $('li#' + songId).find('.song-name').children('p').html();
        $('#song-in-audio-name').children('p').html(songName);

        //delete the finished song from the waiting list
        $('#myAudio').html("");
        $('#myAudio').append("<source id=" + songId +"class='waitingSong' src=" + path + " type='audio/" + extension + "'>");
        $('#myAudio').append(waitingSongs);
        $("#myAudio").trigger('load');
        play_audio('play');
    }
});

//------------------Change song on audio player when previous is finished -----------------------
$('audio').on('ended', function () {
    var waitingSongs = $('#myAudio').children(':not(:first)');
    $('#myAudio').html("");
    $('#myAudio').append(waitingSongs);
    $("#myAudio").trigger('load');
    $("#myAudio").trigger('play');
    var songid = waitingSongs.first().attr('id');
    var songName = $('li#' + songid).find('.song-name').children('p').html();
    $('#song-in-audio-name').children('p').html(songName);
});


//_________________________________________________________ Just to not forget something
$('.add-playlist-to-library').click(function () {
    alert("To DO :add playlist to list of playlists")
});