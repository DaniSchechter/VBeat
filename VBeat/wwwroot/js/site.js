
$(document).ready(function () {
    $('.carousel').carousel();
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
    var songName;
    var length = $('#playlist-song-list').children("li").last().attr('id');
    if (length) {
        //load data to the audio player
        for (i = 0; i <=length; i++) {
            path = $("li#" + i).find('.song-path-container').children('p').html();
            extension = path.split('.').pop();
            songName = $('li#' + i).find('.song-name').children('p').html();
            if (checkSongValidation(path))
                $('#myAudio').append("<source class='" + songName + "' src=" + path + " type='audio/" + extension + "'>");
            else {
                $("li#" + i).children('.playlist-song-container').css("background-color", "red");
                $("li#" + i).children('.playlist-song-container').off('mouseover');
                $("li#" + i).children('.playlist-song-container').off('mouseleave');
                $("li#" + i).find('.song-artist').html('<p style="font-weight:bold;color:black">Type is Not Supported !</p>');
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

function addSongToAudio(path,name) {
    extension = path.split('.').pop();
    var valid = checkSongValidation(path);
    if (valid == "") {
        alert("audio player do not support this type of files");
    }
    else{
        //get the waiting songs in the audio player
        var waitingSongs = $('#myAudio').html();

        //add the new song to the top of the audio songs list
        $('#myAudio').html("");
        $('#myAudio').append("<source class='"+ name + "' src=" + path + " type='audio/" + extension + "'>");
        $('#myAudio').append(waitingSongs);
        $("#myAudio").trigger('load');
        $("#myAudio").trigger('play');

        $('#song-in-audio-name').children('p').html(name);
    }
}

//------------------Add song manually to audio player on click from playlist-----------------------
$('.add-song-to-audio-player').click(function () {
    var path = $(this).siblings('.song-path-container').children('p').html();
    extension = path.split('.').pop();
    var valid = checkSongValidation(path);
    if (valid == "") {
        alert("audio player do not support this type of files");
    }
    else {      
        //get the song name and update the p near the audio player, that displays song's name
        var songId = $(this).closest('li').attr('id');
        var songName = $('li#' + songId).find('.song-name').children('p').html();
        addSongToAudio(path, songName);
    }
});

//------------------Change song on audio player when previous is finished -----------------------
$('audio').on('ended', function () {
    var waitingSongs = $('#myAudio').children(':not(:first)');
    var newFirstSong = waitingSongs.first();
    $('#myAudio').html("");
    $('#myAudio').append(waitingSongs);
    $("#myAudio").trigger('load');
    $("#myAudio").trigger('play');
    var songid = waitingSongs.first().attr('id');
    var songName = newFirstSong.attr('class');
    //if the audio list finished
    if (songName != null)
        $('#song-in-audio-name').children('p').html(songName);
    else
        $('#song-in-audio-name').children('p').html("");
});

//---------------------------Display  playlist when adding song to playlist------------------- 
$('#add-song-to-playlist').click(function () {
    $('.add-song-to-specific-playlist').show();
});
//----------------------------Add songs to audio player from song display page RECENTLY PLAYED------------------
$('.frontpage_square').mouseover(function () {
    $(this).find('img').show();
});
$('.frontpage_square').mouseleave(function () {
    $(this).find('img').hide();
});
$('.frontpage_square a').click(function () {
    path = $(this).parent().closest('.frontpage_square').siblings().find('p').html();
    addSongToAudio(path);
    /*ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERROR ERRORERROR ERRORERROR ERROR ERROR ERROR ERROR ERROR  ERROR ERROR ERRORERROR v v*/
});
//----------------------------Add songs to audio player from song display page NEW RELEASES------------------
$('.item a').click(function () {
    path = $(this).siblings().children('p').html();
    var valid = checkSongValidation(path);
    if (valid == "") {
        alert("audio player do not support this type of files");
    }
    else {
        //get the song name and update the p near the audio player, that displays song's name
        var songName = $(this).siblings('div.carousel-caption').children('h3').html();
        addSongToAudio(path, songName);
    }

});
//----------------------------Add songs to audio player from song Index/AllSongs/Search------------------
$('.play-song-from-index a').click(function () {
    var path = $(this).siblings('div.song-path-container').children('p').html();
    var songName = $(this).closest('td').prev('td').find('h1').html();
    addSongToAudio(path, songName);
});

//----------------------------Add artists to songs------------------
$('.add-artist-to-song select').change(function () {

    if ($(this).children('option:selected').attr('value') != "") {
        //disable the option to select more then once
        $(this).children('option:selected').attr("disabled", true);
        var artistName = $(this).children('option:selected').html();
        var artistID = $(this).children('option:selected').attr('value');
        var songId = $('.song-id').html();
        $('.added-artists-to-song').append("<div class='playlist-song-container " + artistID +"'><p style='padding-left:5%;float:left;font-size:3vh;width=80%;'>" + artistName + "</p> <img class='delete-item' style='padding:1% 5% 0 0;height:80%;position:relative;float:right' src='/images/delete.png' /> </div>");
        $.get({
            url: '/SongModels/addArtistToSong',
            data: { songId: '' + songId, artistId: '' + artistID },
        });
    }
    $(this).children().attr("selected", false);
    $(this).children('[value=""]').attr('selected', true);

});

$(".added-artists-to-song").on("click", ".playlist-song-container img.delete-item", function () {
    var songId = $('.song-id').html();
    var artistID = $(this).parent('div.playlist-song-container').attr('class');
    artistID = artistID.split(" ")[1];

    //release the disabled option from the connected select option
    var option = $('.add-artist-to-song select').children("option[value='" + artistID + "']");
    option.attr('disabled', false);

    //remove the artisr from the added artists list
    var artistToBeDeleted = $('.added-artists-to-song div').hasClass(artistID);
    $('.added-artists-to-song div.'+artistID).remove();

    $.get({
        url: '/SongModels/removeArtistFromSong',
        data: { songId: '' + songId, artistId: '' + artistID },
    });

});

//----------------------------Add artists to shows------------------

$('.add-artist-to-show select').change(function () {

    if ($(this).children('option:selected').attr('value') != "") {
        //disable the option to select more then once
        $(this).children('option:selected').attr("disabled", true);
        var artistName = $(this).children('option:selected').html();
        var artistID = $(this).children('option:selected').attr('value');
        var showId = $('.show-id').html();
        $('.added-artists-to-show').append("<div class='playlist-song-container " + artistID + "'><p style='padding-left:5%;float:left;font-size:3vh;width=80%;'>" + artistName + "</p> <img class='delete-item' style='padding:1% 5% 0 0;height:80%;position:relative;float:right' src='/images/delete.png' /> </div>");
        $.get({
            url: '/ShowModels/addArtistToShow',
            data: { showId: '' + showId, artistId: '' + artistID },
        });
    }
    $(this).children().attr("selected", false);
    $(this).children('[value=""]').attr('selected', true);

});

$(".added-artists-to-show").on("click", ".playlist-song-container img.delete-item", function () {
    var showId = $('.show-id').html();
    var artistID = $(this).parent('div.playlist-song-container').attr('class');
    artistID = artistID.split(" ")[1];

    //release the disabled option from the connected select option
    var option = $('.add-artist-to-show select').children("option[value='" + artistID + "']");
    option.attr('disabled', false);

    //remove the artisr from the added artists list
    var artistToBeDeleted = $('.added-artists-to-show div').hasClass(artistID);
    $('.added-artists-to-show div.' + artistID).remove();

    $.get({
        url: '/ShowModels/removeArtistFromShow',
        data: { showId: '' + showId, artistId: '' + artistID },
    });

});