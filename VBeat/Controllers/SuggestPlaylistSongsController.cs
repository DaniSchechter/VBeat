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
            PlaylistModel playlistModel = _context.Playlists.Where(p => p.PlaylistId == playlistId).FirstOrDefault();
            if (playlistModel == null)
            {
                return NotFound();
            }

            _algo.Train(_context.Playlists.Where(p => p.PlaylistId != playlistId).ToList());

            return View("~/Views/PlaylistModels/SuggestView.cshtml");
        }
    }
}