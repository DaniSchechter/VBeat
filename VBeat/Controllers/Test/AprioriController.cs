using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VBeat.Models;
using VBeat.Models.Algorithm.Learning;

namespace VBeat.Controllers.Test
{
    public class AprioriController : Controller
    {
        AprioriSuggestionAlgorithm suggestionAlgorithm;
        VBeatDbContext _context;

        public AprioriController()
        {
            _context = new VBeatDbContext();
            suggestionAlgorithm = AprioriSuggestionAlgorithm.GetInstance();
        }


        public List<int> Test()
        {
            SongModel sharedSong = new SongModel();
            SongModel privateSong = new SongModel();
            sharedSong.SongId = 1000;
            privateSong.SongId = 500;

            PlaylistModel playlistModel = new PlaylistModel();
            playlistModel.PlaylistId = 1;
            playlistModel.Songs.Add(new Models.BridgeModel.PlaylistSongModel() { PlaylistId = playlistModel.PlaylistId, SongId = sharedSong.SongId });
            playlistModel.Songs.Add(new Models.BridgeModel.PlaylistSongModel() { PlaylistId = playlistModel.PlaylistId, SongId = privateSong.SongId });

            PlaylistModel secondPlaylistModel = new PlaylistModel();
            secondPlaylistModel.PlaylistId = 2;
            secondPlaylistModel.Songs.Add(new Models.BridgeModel.PlaylistSongModel() { PlaylistId = secondPlaylistModel.PlaylistId, SongId = sharedSong.SongId });
            secondPlaylistModel.Songs.Add(new Models.BridgeModel.PlaylistSongModel() { PlaylistId = secondPlaylistModel.PlaylistId, SongId = privateSong.SongId });

            PlaylistModel targetPlaylistModel = new PlaylistModel();
            targetPlaylistModel.PlaylistId = 3;
            targetPlaylistModel.Songs.Add(new Models.BridgeModel.PlaylistSongModel() {  PlaylistId = targetPlaylistModel.PlaylistId, SongId = sharedSong.SongId});

            suggestionAlgorithm.Train(new List<PlaylistModel>()
            {
                playlistModel,
                secondPlaylistModel
            });

            List<int> songs = suggestionAlgorithm.Suggset(targetPlaylistModel);
            return songs;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}