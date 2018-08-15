using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VBeat.Models;
using VBeat.Models.Algorithm.Learning;

namespace VBeat.Controllers.PlaylistModels
{
    public class SuggestPlaylistSongsController : Controller
    {
        VBeatDbContext _context;
        AprioriSuggestionAlgorithm _algo;

        public SuggestPlaylistSongsController()
        {
            _context = new VBeatDbContext();
            _algo = AprioriSuggestionAlgorithm.GetInstance();
        }

        public IActionResult Suggest(int playlistId)
        {
            PlaylistModel targetPlaylistModel = _context.Playlists.Where(p => p.PlaylistId == playlistId).FirstOrDefault();
            if (targetPlaylistModel == null)
            {
                return NotFound();
            }

            _algo.Train(_context.Playlists.Where(p => p.PlaylistId != playlistId).ToList());
            List<int> songList = _algo.Suggset(targetPlaylistModel);
            List<SongModel> songModels = _context.Songs.Where(s => songList.Contains(s.SongId)).ToList();

            ViewData["SongList"] = songModels;
            ViewData["Target"] = targetPlaylistModel;
            return View("~/Views/PlaylistModels/SuggestView.cshtml");
        }
    }
}